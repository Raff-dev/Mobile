# Generated by Django 3.2 on 2021-05-21 15:11

from django.db import migrations


class Migration(migrations.Migration):

    dependencies = [
        ('profiles', '0005_alter_skinownership_options'),
    ]

    operations = [
        migrations.AlterModelOptions(
            name='skin',
            options={'ordering': ['name']},
        ),
        migrations.AlterModelOptions(
            name='skinownership',
            options={'ordering': ['profile', 'skin__name']},
        ),
    ]
