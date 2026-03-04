import { jwtDecode } from "jwt-decode"

type JwtPayload = Record<string, unknown>

const ROLE_CLAIM = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
const NAMEID_CLAIM =
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"

export function setToken(token: string) {
  localStorage.setItem("auth_token", token)
}

export function clearToken() {
  localStorage.removeItem("auth_token")
}

export function getToken(): string | null {
  return localStorage.getItem("auth_token")
}

export function getUserIdFromToken(token: string): string | null {
  try {
    const p = jwtDecode<JwtPayload>(token)
    const sub = p["sub"]
    const nameId = p[NAMEID_CLAIM]
    if (typeof sub === "string" && sub) return sub
    if (typeof nameId === "string" && nameId) return nameId
    return null
  } catch {
    return null
  }
}

export function getRolesFromToken(token: string): string[] {
  try {
    const p = jwtDecode<JwtPayload>(token)
    const raw = p[ROLE_CLAIM] ?? p["role"] ?? p["roles"]
    if (Array.isArray(raw)) return raw.map(String)
    if (typeof raw === "string") return [raw]
    return []
  } catch {
    return []
  }
}

