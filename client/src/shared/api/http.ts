import { ApiError } from "./types"

const API_PREFIX = "/api"

function getAuthToken(): string | null {
  return localStorage.getItem("auth_token")
}

async function readBody(res: Response): Promise<unknown> {
  const contentType = res.headers.get("content-type") ?? ""
  if (contentType.includes("application/json")) return await res.json()
  return await res.text()
}

export async function apiFetch<T>(
  path: string,
  options: RequestInit & { auth?: boolean } = {},
): Promise<T> {
  const url = `${API_PREFIX}${path.startsWith("/") ? path : `/${path}`}`

  const headers = new Headers(options.headers)
  headers.set("Accept", "application/json")

  if (!headers.has("Content-Type") && options.body) {
    headers.set("Content-Type", "application/json")
  }

  if (options.auth) {
    const token = getAuthToken()
    if (token) headers.set("Authorization", `Bearer ${token}`)
  }

  const res = await fetch(url, { ...options, headers })
  if (res.ok) return (await readBody(res)) as T

  const payload = await readBody(res).catch(() => undefined)
  const message =
    typeof payload === "object" && payload && "message" in (payload as any)
      ? String((payload as any).message)
      : `Request failed (${res.status})`

  throw new ApiError(message, res.status, payload)
}

