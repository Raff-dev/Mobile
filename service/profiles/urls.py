from django.urls import path, include
from rest_framework_simplejwt.views import TokenRefreshView, TokenVerifyView
from .views import EmailTokenObtainPairView, ProfileViewSet, RegisterViewSet

from rest_framework import routers

router = routers.DefaultRouter()
router.register('Profiles', ProfileViewSet, basename='Profiles')
router.register('Register', RegisterViewSet, basename='Register')

urlpatterns = router.urls + [
    path('token', EmailTokenObtainPairView.as_view(), name='token_obtain_pair'),
    path('token/refresh', TokenRefreshView.as_view(), name='token_refresh'),
    path('token/verify', TokenVerifyView.as_view(), name='token_verify'),
]


for url in router.urls:
    print(url)
