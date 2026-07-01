using LocalizedContent.Data;

namespace LocalizedContent.Endpoints;

public static class Users
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes)
    {
        var users = routes.MapGroup("/api/v1/users");

        users.MapGet("", () => Collections.Users);

        users.MapGet("/{id}", (int id) => Collections.Users
                                                     .FirstOrDefault(user => user.Id == id));

        users.MapPost("", (User user) => Collections.Users.Add(user));

        users.MapPut("/{id}", (int id, User user) =>
        {
            var currentUser = Collections.Users.FirstOrDefault(user => user.Id == id);

            currentUser!.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.BirthDate = user.BirthDate;
        });

        users.MapDelete("/{id}", (int id) =>
        {
            var userForDeletion = Collections.Users.FirstOrDefault(user => user.Id == id);

            Collections.Users.Remove(userForDeletion);
        });
    }
}
