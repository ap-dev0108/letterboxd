import { useQuery } from "@tanstack/react-query"

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { getProfile } from "@/features/users/api"
import { getRolesFromToken, getToken } from "@/shared/auth/token"

export function ProfilePage() {
  const token = getToken()
  const roles = token ? getRolesFromToken(token) : []

  const q = useQuery({
    queryKey: ["profile"],
    queryFn: async () => {
      const res = await getProfile()
      if (!res.success) throw new Error(res.message || "Failed to load profile")
      return res.data
    },
    enabled: Boolean(token),
  })

  if (!token) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>Profile</CardTitle>
          <CardDescription>You are not logged in.</CardDescription>
        </CardHeader>
      </Card>
    )
  }

  return (
    <div className="max-w-xl space-y-4">
      <Card>
        <CardHeader>
          <CardTitle>Your profile</CardTitle>
          <CardDescription>Fetched from `GET /User/user-profile`.</CardDescription>
        </CardHeader>
        <CardContent className="text-sm">
          {q.isLoading && <div className="text-muted-foreground">Loading...</div>}
          {q.isError && (
            <div className="text-destructive">
              {(q.error as Error).message}
            </div>
          )}
          {q.data && (
            <div className="space-y-2">
              <div>
                <div className="text-muted-foreground">Username</div>
                <div className="font-medium">{q.data.userName ?? "-"}</div>
              </div>
              <div>
                <div className="text-muted-foreground">Email</div>
                <div className="font-medium">{q.data.email ?? "-"}</div>
              </div>
              <div>
                <div className="text-muted-foreground">Roles</div>
                <div className="font-medium">{roles.length ? roles.join(", ") : "-"}</div>
              </div>
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  )
}

