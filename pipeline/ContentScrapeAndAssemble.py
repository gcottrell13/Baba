#! py -3.11

from modules.analysis import analyze_images
from modules.make_proofs import make_all_proofs
from modules.output_cs import save_object_info, save_palette_info, output_spritesheets, output_soundmap

from modules.load_object_information import load_information
from modules.imageUtils import open_all_images

if __name__ == "__main__":
    images = open_all_images()
    analysis_result = analyze_images(images, load_information()[0])
    save_object_info()
    save_palette_info()
    output_soundmap()
    output_spritesheets(analysis_result)
    # make_all_proofs(analysis_result)
