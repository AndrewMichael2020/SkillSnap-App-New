# SkillSnap-App-New

## How to run the API

**Before running the API, set the JWT secret key:**

- For development, you can add this to `SkillSnap.Api/appsettings.json`:
  ```json
  "Jwt": {
    "Secret": "your-very-strong-secret-key"
  }
  ```
- Or set it as an environment variable:
  ```
  export Jwt__Secret=your-very-strong-secret-key
  ```

To run the API project, use:

```
dotnet run --project SkillSnap.Api/SkillSnap.Api.csproj
```

To run the Blazor client (if needed):

```
dotnet run --project SkillSnap.Client/SkillSnap.Client.csproj
```