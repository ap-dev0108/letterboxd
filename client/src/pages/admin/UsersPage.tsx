import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { toast } from "sonner"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { deleteUserAdmin, getAllUsersAdmin } from "@/features/admin/users/api"

export function UsersPage() {
  const qc = useQueryClient()

  const q = useQuery({
    queryKey: ["admin-users"],
    queryFn: async () => {
      const res = await getAllUsersAdmin()
      if (!res.success) throw new Error(res.message || "Failed to load users")
      return res.data
    },
  })

  const del = useMutation({
    mutationFn: async (userID: string) => {
      const res = await deleteUserAdmin(userID)
      if (!res.success) throw new Error(res.message || "Failed to delete user")
      return res
    },
    onSuccess: async () => {
      toast.success("User deleted")
      await qc.invalidateQueries({ queryKey: ["admin-users"] })
    },
    onError: (e: any) => toast.error(e?.message ?? "Failed to delete user"),
  })

  return (
    <div className="space-y-4">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Users (Admin)</h1>
        <p className="text-muted-foreground text-sm mt-1">
          Uses `GET /getAllUsers` and `GET /User/delete-user`.
        </p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>All users</CardTitle>
          <CardDescription>
            {q.isLoading ? "Loading..." : q.data ? `${q.data.length} user(s)` : ""}
          </CardDescription>
        </CardHeader>
        <CardContent>
          {q.isError && (
            <div className="text-destructive text-sm">{(q.error as Error).message}</div>
          )}

          <div className="space-y-2">
            {q.data?.map((u) => (
              <div key={u.id} className="rounded-lg border p-3 flex items-center justify-between">
                <div className="text-sm">
                  <div className="font-medium">{u.userName ?? "-"}</div>
                  <div className="text-muted-foreground">{u.email ?? "-"}</div>
                  <div className="text-xs text-muted-foreground mt-1">{u.id}</div>
                </div>
                <Button
                  variant="destructive"
                  disabled={del.isPending}
                  onClick={() => del.mutate(u.id)}
                >
                  Delete
                </Button>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

