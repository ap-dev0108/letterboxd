import { useState, type ChangeEvent } from "react"
import { useNavigate } from "react-router-dom"
import { toast } from "sonner"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { login } from "@/features/auth/api"
import { setToken } from "@/shared/auth/token"

export function LoginPage() {
  const navigate = useNavigate()
  const [usermail, setUsermail] = useState("")
  const [userpassword, setUserpassword] = useState("")
  const [loading, setLoading] = useState(false)

  return (
    <div className="mx-auto max-w-md">
      <Card>
        <CardHeader>
          <CardTitle>Login</CardTitle>
          <CardDescription>Sign in to use protected endpoints.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              value={usermail}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setUsermail(e.target.value)}
              placeholder="you@example.com"
              autoComplete="email"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="password">Password</Label>
            <Input
              id="password"
              type="password"
              value={userpassword}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setUserpassword(e.target.value)}
              autoComplete="current-password"
            />
          </div>

          <Button
            className="w-full"
            disabled={loading}
            onClick={async () => {
              setLoading(true)
              try {
                const res = await login({ usermail, userpassword })
                if (!res.success || !res.data) throw new Error(res.message || "Login failed")
                setToken(res.data)
                toast.success("Logged in")
                navigate("/profile")
              } catch (e: any) {
                toast.error(e?.message ?? "Login failed")
              } finally {
                setLoading(false)
              }
            }}
          >
            {loading ? "Signing in..." : "Sign in"}
          </Button>

          <Button variant="ghost" className="w-full" onClick={() => navigate("/register")}>
            Create an account
          </Button>
        </CardContent>
      </Card>
    </div>
  )
}

