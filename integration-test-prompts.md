# Integration Test Prompts

- Register user via `/api/auth/register`
- Login user and store JWT token
- Attach JWT to Authorization header for subsequent requests
- Attempt to call `[Authorize]` route with and without token
- Verify `401 Unauthorized` is returned for unauthorized access

---

**Note:**  
These prompts describe the integration test scenarios, but they are not executable by GitHub Actions as-is.  
For GitHub Actions, you need a workflow YAML file (e.g., `.github/workflows/integration-tests.yml`) that sets up the environment, runs your API, and executes integration tests (using a tool like `dotnet test`, `curl`, or a test framework).

If you want to automate these tests, you should:
- Implement integration tests in code (e.g., using xUnit, NUnit, or a REST client).
- Create a GitHub Actions workflow YAML to build, run, and test your application.
