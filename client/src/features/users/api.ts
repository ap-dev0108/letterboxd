import { apiFetch } from "@/shared/api/http"
import type { ApiResponse } from "@/shared/api/types"

export type UserProfile = {
  userName?: string
  email?: string
  userID?: string
}

export async function getProfile() {
  return await apiFetch<ApiResponse<UserProfile>>("/User/user-profile", {
    method: "GET",
    auth: true,
  })
}

