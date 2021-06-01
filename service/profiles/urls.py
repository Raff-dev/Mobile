from django.urls import path
from rest_framework import routers

from .views import ProfileViewSet, SkinViewSet, verify

router = routers.DefaultRouter()
router.register('Profiles', ProfileViewSet, basename='Profiles')
router.register('Skins', SkinViewSet, basename='Skins')

urlpatterns = router.urls + [
    path('activate/<slug:uid>/<slug:token>/', verify)
]
