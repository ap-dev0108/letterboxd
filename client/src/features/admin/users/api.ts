import { apiFetch } from "@/shared/api/http"
import type { ApiResponse } from "@/shared/api/types"

export type AdminUser = {
  id: string
  userName?: string | null
  email?: string | null
}

export async function getAllUsersAdmin() {
  return await apiFetch<ApiResponse<AdminUser[]>>("/getAllUsers", { method: "GET", auth: true })
}

export async function deleteUserAdmin(userID: string) {
  const qs = new URLSearchParams({ userID })
  return await apiFetch<ApiResponse<unknown>>(`/User/delete-user?${qs.toString()}`, {
    method: "GET",
    auth: true,
  })
}

