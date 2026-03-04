import { apiFetch } from "@/shared/api/http"
import type { ApiResponse } from "@/shared/api/types"

export async function checkLogin() {
  return await apiFetch<ApiResponse<string>>("/checkLogin", { method: "GET", auth: true })
}

export type RawUser = Record<string, unknown>

export async function getUserById(userID: string) {
  const qs = new URLSearchParams({ userID })
  return await apiFetch<ApiResponse<RawUser>>(`/getUsers?${qs.toString()}`, { method: "GET" })
}

