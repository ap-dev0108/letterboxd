import { apiFetch } from "@/shared/api/http"
import type { ApiResponse } from "@/shared/api/types"

export type LoginRequest = { usermail: string; userpassword: string }
export type RegisterRequest = {
  userName: string
  email: string
  userPass: string
}

export async function login(req: LoginRequest) {
  return await apiFetch<ApiResponse<string>>("/login", {
    method: "POST",
    body: JSON.stringify(req),
  })
}

export async function register(req: RegisterRequest) {
  return await apiFetch<ApiResponse<{ userName?: string }>>("/register", {
    method: "POST",
    body: JSON.stringify(req),
  })
}

