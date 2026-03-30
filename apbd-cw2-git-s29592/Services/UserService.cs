using apbd_cw2_git_s29592.Domain.Users;

namespace apbd_cw2_git_s29592.Services;

public class UserService
{
    private readonly List<User> _users = [];

    public void Add(User user) => _users.Add(user);

    public IReadOnlyList<User> GetAll() => _users.AsReadOnly();

    public User? FindById(Guid id) =>
        _users.FirstOrDefault(u => u.Id == id);
}