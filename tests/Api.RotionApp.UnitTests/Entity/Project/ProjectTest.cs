
using Api.RotionApp.Domain.Exceptions;
using System;
using Xunit;

using DomainEntity = Api.RotionApp.Domain.Entity;

namespace Api.RotionApp.UnitTests.Entity.Project;
public class ProjectTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Project - Aggregates")]
    public void Instantiate()
    {
        // Arrange
        var validData = new
        {
            Title = "project name",
            Description = "Description",
            Emoji = "emoji"
        };

        // Act
        var dateTimeBefore = DateTime.Now;
        var project = new DomainEntity.Project(validData.Title, validData.Description, validData.Emoji);
        var dateTimeAfter = DateTime.Now;

        // Assert
        Assert.NotNull(project);
        Assert.Equal(validData.Title, project.Title);
        Assert.Equal(validData.Description, project.Description);
        Assert.Equal(validData.Emoji, project.Emoji);
        Assert.NotEqual(default(Guid), project.Id);
        Assert.NotEqual(default(DateTime), project.CreatedAt);
        Assert.True(project.CreatedAt > dateTimeBefore);
        Assert.True(project.CreatedAt < dateTimeAfter);

    }

    [Theory(DisplayName = nameof(Instantiate_ErrorWhenTitleIsEmpty))]
    [Trait("Domain", "Project - Aggregates")]
    [InlineData(null)]
    public void Instantiate_ErrorWhenTitleIsEmpty(string? title)
    {
        Action action = () => new DomainEntity.Project(title!, "description", "emoji");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Title should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(Instantiate_ErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Project - Aggregates")]
    public void Instantiate_ErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Project("project title", null!, "emoji");

        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Fact(DisplayName = nameof(Instantiate_ErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Project - Aggregates")]
    public void Instantiate_ErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

        Action action = () => new DomainEntity.Project("title", invalidDescription, "emoji");
        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should not be greater than 10_000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Project - Aggregates")]
    public void Update()
    {
        var project = new DomainEntity.Project("Project name", "project description", "project emoji");
        var newValues = new { Title = "New title", Description = "new Description", Emoji = "New Emoji" };

        project.Update(newValues.Title, newValues.Description, newValues.Emoji);

        Assert.Equal(newValues.Title, project.Title);
        Assert.Equal(newValues.Description, project.Description);
        Assert.Equal(newValues.Emoji, project.Emoji);
    }

    [Fact(DisplayName = nameof(Update_OnlyTitle))]
    [Trait("Domain", "Project - Aggregates")]
    public void Update_OnlyTitle()
    {
        

        var project = new DomainEntity.Project("Project name", "project description", "project emoji");
        var newValues = new { Title = "New title" };

        var currentDescription = project.Description;
        var currentEmoji = project.Emoji;

        project.Update(newValues.Title);

        Assert.Equal(newValues.Title, project.Title);
        Assert.Equal(currentDescription, project.Description);
        Assert.Equal(currentEmoji, project.Emoji);
    }

    [Theory(DisplayName = nameof(UpdateTitle_WhenNewTitleIsNull_ShouldRetainPreviousTitle))]
    [Trait("Domain", "Project - Aggregates")]
    [InlineData(null)]
    public void UpdateTitle_WhenNewTitleIsNull_ShouldRetainPreviousTitle(string? title)
    {
        var project = new DomainEntity.Project("Project name", "project description", "project emoji");
        var currentTitle = project.Title;
        project.Update(null!);

        Assert.Equal(project.Title, currentTitle);
    }

    [Fact(DisplayName = nameof(Update_OnlyDescription))]
    [Trait("Domain", "Project - Aggregates")]
    public void Update_OnlyDescription()
    {
        var project = new DomainEntity.Project("Project name", "project description", "project emoji");
        var newValues = new { Description = "New Description" };
        var currentTitle = project.Title;
        var currentEmoji = project.Emoji;

        project.Update(null!, newValues.Description, null!);

        Assert.Equal(newValues.Description, project.Description);
        Assert.Equal(currentTitle, project.Title);
        Assert.Equal(currentEmoji, project.Emoji);
    }

    [Fact(DisplayName = nameof(Update_ErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Project - Aggregates")]
    public void Update_ErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var project = new DomainEntity.Project("Project name", "project description", "project emoji");

        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

        Action action = () => project.Update(null!, invalidDescription, null!);
        var exception = Assert.Throws<EntityValidationException>(action);

        Assert.Equal("Description should not be greater than 10_000 characters long", exception.Message);
    }
}
