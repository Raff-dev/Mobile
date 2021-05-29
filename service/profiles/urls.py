from django.urls import path
from rest_framework import routers

from .views import ProfileViewSet, verify

router = routers.DefaultRouter()
router.register('Profiles', ProfileViewSet, basename='Profiles')

urlpatterns = router.urls + [
    path('activate/<slug:uid>/<slug:token>/', verify)
]
