using MyShop.Core.Exceptions;
using MyShop.Core.Models.Notifications;
using MyShop.Core.Models.Photos;
using MyShop.Core.Models.Products;
using MyShop.Core.ValueObjects.Shared;
using MyShop.Core.ValueObjects.Users;

namespace MyShop.Core.Models.Users;
public abstract class RegisteredUser : User
{
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public UserPhoneNumber PhoneNumber { get; private set; }
    public Gender Gender { get; private set; }
    public string SecuredPassword { get; private set; }
    public DateOfBirth DateOfBirth { get; private set; }
    public UserPhoto? Photo { get; private set; }
    public Guid? PhotoId { get; private set; }
    public IReadOnlyCollection<Notification> Notifications { get; private set; } = default!;
    public IReadOnlyCollection<NotificationRegisteredUser> NotificationUsers { get; private set; } = default!;
    public IReadOnlyCollection<UserAddress> UserAddresses { get; private set; } = default!;
    public IReadOnlyCollection<ProductReview> ProductReviews { get; private set; } = default!;
    public IReadOnlyCollection<Favorite> Favorites { get; private set; } = default!;

    public RegisteredUser(
        UserRole userRole,
        FirstName firstName,
        LastName lastName,
        Email email,
        Gender gender,
        string securedPassword,
        DateOfBirth dateOfBirth,
        UserPhoneNumber phoneNumber
        ) : base(userRole)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(securedPassword, nameof(securedPassword));

        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Gender = gender ?? throw new ArgumentNullException(nameof(gender));
        SecuredPassword = securedPassword ?? throw new ArgumentNullException(nameof(securedPassword));
        DateOfBirth = dateOfBirth ?? throw new ArgumentNullException(nameof(dateOfBirth));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
    }

    public void Update(
        FirstName firstName,
        LastName lastName,
        Gender gender,
        DateOfBirth dateOfBirth,
        UserPhoneNumber phoneNumber
        )
    {

        if (
            FirstName == firstName &&
            LastName == lastName &&
            Gender == gender &&
            DateOfBirth == dateOfBirth &&
            PhoneNumber == phoneNumber
            )
        {
            throw new BadRequestException("Nothing change.");
        }

        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        Gender = gender ?? throw new ArgumentNullException(nameof(gender));
        DateOfBirth = dateOfBirth ?? throw new ArgumentNullException(nameof(dateOfBirth));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
    }

    public void UpdateEmail(Email email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public void UpdateSecuredPassword(string securedPassword)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(securedPassword, nameof(securedPassword));
        SecuredPassword = securedPassword;
    }

    public void SetPhoto(UserPhoto? photo)
    {
        Photo = photo;
        PhotoId = photo?.Id;
    }
}
