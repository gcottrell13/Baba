from babaparse import analyze_images, open_all_images, create_tileset



images = open_all_images()
analyze_images(images)
create_tileset(images)