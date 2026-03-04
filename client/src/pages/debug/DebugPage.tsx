import { useState, type ChangeEvent } from "react"
import { useMutation } from "@tanstack/react-query"
import { toast } from "sonner"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { checkLogin, getUserById } from "@/features/debug/api"

export function DebugPage() {
  const [userID, setUserID] = useState("")
  const [userJson, setUserJson] = useState<string>("")

  const checkM = useMutation({
    mutationFn: async () => {
      const res = await checkLogin()
      if (!res.success) throw new Error(res.message || "Not logged in")
      return res.data
    },
    onSuccess: () => toast.success("Token was sent in Authorization header"),
    onError: (e: any) => toast.error(e?.message ?? "Failed"),
  })

  const userM = useMutation({
    mutationFn: async () => {
      const id = userID.trim()
      if (!id) throw new Error("userID is required")
      const res = await getUserById(id)
      if (!res.success) throw new Error(res.message || "Failed to fetch user")
      return res.data
    },
    onSuccess: (data) => setUserJson(JSON.stringify(data, null, 2)),
    onError: (e: any) => toast.error(e?.message ?? "Failed"),
  })

  return (
    <div className="space-y-4">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Debug</h1>
        <p className="text-muted-foreground text-sm mt-1">
          Mirrors `AuthController` endpoints.
        </p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Check login</CardTitle>
          <CardDescription>Calls `GET /checkLogin` with Authorization header.</CardDescription>
        </CardHeader>
        <CardContent>
          <Button disabled={checkM.isPending} onClick={() => checkM.mutate()}>
            {checkM.isPending ? "Checking..." : "Check"}
          </Button>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Get user</CardTitle>
          <CardDescription>Calls `GET /getUsers?userID=...`.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-3">
          <div className="space-y-2">
            <Label>UserID</Label>
            <Input
              value={userID}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setUserID(e.target.value)}
              placeholder="identity user id"
            />
          </div>
          <Button disabled={userM.isPending} onClick={() => userM.mutate()}>
            {userM.isPending ? "Loading..." : "Fetch"}
          </Button>
          {userJson && (
            <pre className="text-xs rounded-lg border p-3 overflow-auto max-h-[320px]">
{userJson}
            </pre>
          )}
        </CardContent>
      </Card>
    </div>
  )
}

