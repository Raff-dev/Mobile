from django.db import models
from django.db.models.fields.related import ForeignKey
from django.utils.translation import ugettext_lazy as _
from django.contrib.auth.base_user import BaseUserManager
from django.contrib.auth.models import AbstractBaseUser, PermissionsMixin


class ProfileManager(BaseUserManager):
    """
    Custom user model manager where email is the unique identifiers
    for authentication instead of usernames.
    """

    def create_user(self, email, password, **extra_fields):
        """
        Create and save a User with the given email and password.
        """
        if not email:
            raise ValueError(_('The Email must be set'))
        email = self.normalize_email(email)
        user = self.model(email=email, **extra_fields)
        user.set_password(password)
        user.save()
        return user

    def create_superuser(self, email, password, **extra_fields):
        """
        Create and save a SuperUser with the given email and password.
        """
        extra_fields.setdefault('is_staff', True)
        extra_fields.setdefault('is_superuser', True)
        extra_fields.setdefault('is_active', True)

        if extra_fields.get('is_staff') is not True:
            raise ValueError(_('Superuser must have is_staff=True.'))
        if extra_fields.get('is_superuser') is not True:
            raise ValueError(_('Superuser must have is_superuser=True.'))
        return self.create_user(email, password, **extra_fields)


class Profile(AbstractBaseUser, PermissionsMixin):
    email = models.EmailField(_('email address'), unique=True)
    is_active = models.BooleanField(default=True)
    is_staff = models.BooleanField(default=False)
    stellar_points = models.IntegerField(default=0)
    high_score = models.IntegerField(default=0)

    USERNAME_FIELD = 'email'
    REQUIRED_FIELDS = []

    objects = ProfileManager()

    def save(self, *args, **kwargs) -> None:
        created = self.id is None
        super().save(*args, **kwargs)

        if created:
            deafault_skin = Skin.objects.filter(default=True).first()
            if deafault_skin:
                SkinOwnership.objects.create(profile=self, skin=deafault_skin)

    class Meta:
        ordering = ['email']

    def __str__(self):
        return self.email


class Skin(models.Model):
    name = models.CharField(max_length=50, unique=True)
    default = models.BooleanField(default=False)

    def __str__(self) -> str:
        return f'{self.name}'


class SkinOwnership(models.Model):
    profile = ForeignKey(Profile, verbose_name=_("Profile"), related_name='skins', on_delete=models.CASCADE)
    skin = models.ForeignKey(Skin, verbose_name=_("Skin"), related_name='ownerships', on_delete=models.CASCADE)

    def __str__(self) -> str:
        return f'{self.profile} - {self.skin.name}'

    class Meta:
        unique_together = ['profile', 'skin']

    @property
    def name(self):
        return f'{self.skin.name}'
