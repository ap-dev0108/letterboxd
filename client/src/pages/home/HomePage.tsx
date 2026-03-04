import { Link } from "react-router-dom"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"

export function HomePage() {
  return (
    <div className="space-y-6">
      <div className="flex items-start justify-between gap-4">
        <div>
          <h1 className="text-3xl font-semibold tracking-tight">Movie app</h1>
          <p className="text-muted-foreground mt-1">
            Browse, filter, and rate movies using your existing .NET API.
          </p>
        </div>
        <Button asChild>
          <Link to="/movies">Open movies</Link>
        </Button>
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        <Card>
          <CardHeader>
            <CardTitle>Movies</CardTitle>
            <CardDescription>List + filter endpoint</CardDescription>
          </CardHeader>
          <CardContent className="text-sm text-muted-foreground">
            Uses `GET /Film/filter-movies` and `GET /getAllMovies` (admin).
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle>Auth</CardTitle>
            <CardDescription>Register + login</CardDescription>
          </CardHeader>
          <CardContent className="text-sm text-muted-foreground">
            Uses `POST /register` and `POST /login` and stores JWT locally.
          </CardContent>
        </Card>
        <Card>
          <CardHeader>
            <CardTitle>Ratings</CardTitle>
            <CardDescription>User reviews feature</CardDescription>
          </CardHeader>
          <CardContent className="text-sm text-muted-foreground">
            Uses `/Rating/*` endpoints to add and view ratings.
          </CardContent>
        </Card>
      </div>
    </div>
  )
}

