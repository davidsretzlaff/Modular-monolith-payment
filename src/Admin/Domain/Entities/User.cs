using Shared.Core;

namespace Admin.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string Name { get; private set; }
    public bool IsActive { get; private set; }
    public Guid CompanyId { get; private set; }
    public DateTime CreateDate { get; private set; }
    public DateTime? LastLoginDate { get; private set; }

    // Propriedade de navegação
    public virtual Company Company { get; private set; } = null!;

    protected User() { } // Para o EF

    public User(
        string email,
        string passwordHash,
        string name,
        Guid companyId)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("PasswordHash cannot be empty", nameof(passwordHash));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        if (companyId == Guid.Empty)
            throw new ArgumentException("CompanyId cannot be empty", nameof(companyId));

        Email = email.ToLowerInvariant();
        PasswordHash = passwordHash;
        Name = name;
        CompanyId = companyId;
        IsActive = true;
        CreateDate = DateTime.UtcNow;
    }

    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty", nameof(newName));

        Name = newName;
        UpdateTimestamp();
    }

    public void UpdatePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("PasswordHash cannot be empty", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
        UpdateTimestamp();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    public void UpdateLastLoginDate()
    {
        LastLoginDate = DateTime.UtcNow;
        UpdateTimestamp();
    }
} 