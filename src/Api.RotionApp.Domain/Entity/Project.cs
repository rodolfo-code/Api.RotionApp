using Api.RotionApp.Domain.Exceptions;

namespace Api.RotionApp.Domain.Entity;
public class Project
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public string? Emoji { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Project(string title, string? description, string? emoji)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Emoji = emoji;
        CreatedAt = DateTime.Now;

        Validate();
    }

    public void Update(string? title = null, string? description = null, string? emoji = null)
    {
        Title = title ?? Title;
        Description = description ?? Description;
        Emoji = emoji ?? Emoji;

        Validate();
    }

    public void Validate()
    {
        if (Title is null)
            throw new EntityValidationException($"{nameof(Title)} should not be empty or null");
        if (Description is null)
            throw new EntityValidationException($"{nameof(Description)} should not be null");
        if (Description.Length > 10_000)
            throw new EntityValidationException($"{nameof(Description)} should not be greater than 10_000 characters long");
    }
}
