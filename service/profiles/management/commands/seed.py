import os

from django.core.management.base import BaseCommand
from ...models import Skin, SkinOwnership


class Command(BaseCommand):
    help = "seed database for testing and development."
    MODELS = [Skin]

    SKIN_NAME = 'StarSparrow'
    FIRST_SKIN = 1
    LAST_SKIN = 13

    def handle(self, *args, **options):
        self.erase_data()
        self.seed_data()

    def erase_data(self):
        for model in self.MODELS:
            model.objects.all().delete()

    def seed_data(self):
        for index in range(self.FIRST_SKIN, self.LAST_SKIN + 1):
            name = self.SKIN_NAME + str(index)
            default = (index == self.FIRST_SKIN)
            Skin.objects.create(name=name, default=default)
