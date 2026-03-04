import { useState, type ChangeEvent } from "react"
import { useNavigate } from "react-router-dom"
import { toast } from "sonner"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { register } from "@/features/auth/api"

export function RegisterPage() {
  const navigate = useNavigate()
  const [userName, setUserName] = useState("")
  const [email, setEmail] = useState("")
  const [userPass, setUserPass] = useState("")
  const [loading, setLoading] = useState(false)

  return (
    <div className="mx-auto max-w-md">
      <Card>
        <CardHeader>
          <CardTitle>Create account</CardTitle>
          <CardDescription>Register via `POST /register`.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="username">Username</Label>
            <Input
              id="username"
              value={userName}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setUserName(e.target.value)}
              autoComplete="username"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="email">Email</Label>
            <Input
              id="email"
              type="email"
              value={email}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)}
              autoComplete="email"
            />
          </div>
          <div className="space-y-2">
            <Label htmlFor="password">Password</Label>
            <Input
              id="password"
              type="password"
              value={userPass}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setUserPass(e.target.value)}
              autoComplete="new-password"
            />
          </div>

          <Button
            className="w-full"
            disabled={loading}
            onClick={async () => {
              setLoading(true)
              try {
                const res = await register({ userName, email, userPass })
                if (!res.success) throw new Error(res.message || "Register failed")
                toast.success("Registered. Now login.")
                navigate("/login")
              } catch (e: any) {
                toast.error(e?.message ?? "Register failed")
              } finally {
                setLoading(false)
              }
            }}
          >
            {loading ? "Creating..." : "Create account"}
          </Button>
        </CardContent>
      </Card>
    </div>
  )
}

