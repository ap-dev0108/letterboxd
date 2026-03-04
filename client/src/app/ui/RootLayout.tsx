import { Link, NavLink, Outlet, useNavigate } from "react-router-dom"
import { LogOut, User } from "lucide-react"

import { Button } from "@/components/ui/button"
import { getRolesFromToken, getToken, clearToken } from "@/shared/auth/token"

function navLinkClass({ isActive }: { isActive: boolean }) {
  return isActive
    ? "text-foreground font-medium"
    : "text-muted-foreground hover:text-foreground"
}

export function RootLayout() {
  const navigate = useNavigate()
  const token = getToken()
  const roles = token ? getRolesFromToken(token) : []
  const isAdmin = roles.includes("Admin")

  return (
    <div className="min-h-screen">
      <header className="border-b bg-background/70 backdrop-blur">
        <div className="mx-auto max-w-6xl px-4 py-3 flex items-center justify-between">
          <Link to="/" className="font-semibold tracking-tight">
            Movie
          </Link>

          <nav className="flex items-center gap-5 text-sm">
            <NavLink to="/movies" className={navLinkClass}>
              Movies
            </NavLink>
            <NavLink to="/genres" className={navLinkClass}>
              Genres
            </NavLink>
            <NavLink to="/ratings" className={navLinkClass}>
              Ratings
            </NavLink>
            {isAdmin && (
              <NavLink to="/admin/users" className={navLinkClass}>
                Users
              </NavLink>
            )}
            <NavLink to="/debug" className={navLinkClass}>
              Debug
            </NavLink>
            {token && (
              <NavLink to="/profile" className={navLinkClass}>
                Profile
              </NavLink>
            )}
            {isAdmin && (
              <span className="text-xs rounded-full border px-2 py-1 text-muted-foreground">
                Admin
              </span>
            )}
          </nav>

          <div className="flex items-center gap-2">
            {!token ? (
              <>
                <Button variant="ghost" onClick={() => navigate("/login")}>
                  Login
                </Button>
                <Button onClick={() => navigate("/register")}>Register</Button>
              </>
            ) : (
              <>
                <Button
                  variant="outline"
                  onClick={() => navigate("/profile")}
                  className="gap-2"
                >
                  <User className="h-4 w-4" /> Account
                </Button>
                <Button
                  variant="ghost"
                  onClick={() => {
                    clearToken()
                    navigate("/")
                  }}
                  className="gap-2"
                >
                  <LogOut className="h-4 w-4" /> Logout
                </Button>
              </>
            )}
          </div>
        </div>
      </header>

      <main className="mx-auto max-w-6xl px-4 py-8">
        <Outlet />
      </main>
    </div>
  )
}

