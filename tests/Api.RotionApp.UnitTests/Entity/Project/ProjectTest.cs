
using Api.RotionApp.Domain.Exceptions;
using FluentAssertions;
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
        project.Should().NotBeNull();
        project.Title.Should().Be(validData.Title);
        project.Description.Should().Be(validData.Description);
        project.Emoji.Should().Be(validData.Emoji);
        project.Id.Should().NotBeEmpty();
        project.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
        (project.CreatedAt > dateTimeBefore).Should().BeTrue();
        (project.CreatedAt < dateTimeAfter).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(Instantiate_ErrorWhenTitleIsEmpty))]
    [Trait("Domain", "Project - Aggregates")]
    [InlineData(null)]
    public void Instantiate_ErrorWhenTitleIsEmpty(string? title)
    {
        Action action = () => new DomainEntity.Project(title!, "description", "emoji");

        var exception = Assert.Throws<EntityValidationException>(action);

        exception.Message.Should().Be("Title should not be empty or null");
    }

    [Fact(DisplayName = nameof(Instantiate_ErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Project - Aggregates")]
    public void Instantiate_ErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Project("project title", null!, "emoji");

        var exception = Assert.Throws<EntityValidationException>(action);
        exception.Message.Should().Be("Description should not be null");
    }

    [Fact(DisplayName = nameof(Instantiate_ErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Project - Aggregates")]
    public void Instantiate_ErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

        Action action = () => new DomainEntity.Project("title", invalidDescription, "emoji");
        var exception = Assert.Throws<EntityValidationException>(action);

        exception.Message.Should().Be("Description should not be greater than 10_000 characters long");
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Project - Aggregates")]
    public void Update()
    {
        var project = new DomainEntity.Project("Project name", "project description", "project emoji");
        var newValues = new { Title = "New title", Description = "new Description", Emoji = "New Emoji" };

        project.Update(newValues.Title, newValues.Description, newValues.Emoji);

        project.Title.Should().Be(newValues.Title);
        project.Description.Should().Be(newValues.Description);
        project.Emoji.Should().Be(newValues.Emoji);
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
