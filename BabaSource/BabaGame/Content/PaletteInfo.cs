
using Microsoft.Xna.Framework; 
using System.Collections.Generic;

namespace BabaGame.Content {
    public static class PaletteInfo {
        private const int shift = 3;
		private static readonly Dictionary<int, Color> palette_abstract = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(70, 87, 56) },
			{ (0 << shift) + 1, new Color(111, 139, 91) },
			{ (0 << shift) + 2, new Color(162, 184, 147) },
			{ (0 << shift) + 3, new Color(235, 244, 226) },
			{ (0 << shift) + 4, new Color(26, 37, 14) },
			{ (1 << shift) + 0, new Color(50, 60, 22) },
			{ (1 << shift) + 1, new Color(67, 76, 32) },
			{ (1 << shift) + 2, new Color(68, 167, 110) },
			{ (1 << shift) + 3, new Color(101, 205, 168) },
			{ (1 << shift) + 4, new Color(147, 220, 202) },
			{ (2 << shift) + 0, new Color(77, 65, 20) },
			{ (2 << shift) + 1, new Color(113, 83, 31) },
			{ (2 << shift) + 2, new Color(167, 111, 37) },
			{ (2 << shift) + 3, new Color(201, 180, 59) },
			{ (2 << shift) + 4, new Color(223, 237, 135) },
			{ (3 << shift) + 0, new Color(85, 73, 88) },
			{ (3 << shift) + 1, new Color(119, 111, 110) },
			{ (3 << shift) + 2, new Color(53, 130, 117) },
			{ (3 << shift) + 3, new Color(67, 166, 158) },
			{ (3 << shift) + 4, new Color(229, 208, 90) },
			{ (4 << shift) + 0, new Color(106, 78, 63) },
			{ (4 << shift) + 1, new Color(150, 96, 80) },
			{ (4 << shift) + 2, new Color(198, 162, 128) },
			{ (4 << shift) + 3, new Color(50, 106, 77) },
			{ (4 << shift) + 4, new Color(98, 191, 205) },
			{ (5 << shift) + 0, new Color(56, 65, 42) },
			{ (5 << shift) + 1, new Color(82, 96, 57) },
			{ (5 << shift) + 2, new Color(120, 132, 72) },
			{ (5 << shift) + 3, new Color(173, 178, 102) },
			{ (5 << shift) + 4, new Color(209, 227, 113) },
			{ (6 << shift) + 0, new Color(91, 104, 39) },
			{ (6 << shift) + 1, new Color(136, 150, 49) },
			{ (6 << shift) + 2, new Color(166, 190, 70) },
			{ (6 << shift) + 3, new Color(54, 66, 31) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_autumn = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(36, 36, 36) },
			{ (0 << shift) + 1, new Color(137, 114, 93) },
			{ (0 << shift) + 2, new Color(195, 195, 195) },
			{ (0 << shift) + 3, new Color(253, 250, 236) },
			{ (0 << shift) + 4, new Color(57, 45, 36) },
			{ (1 << shift) + 0, new Color(82, 64, 40) },
			{ (1 << shift) + 1, new Color(125, 103, 74) },
			{ (1 << shift) + 2, new Color(75, 149, 102) },
			{ (1 << shift) + 3, new Color(74, 176, 140) },
			{ (1 << shift) + 4, new Color(107, 199, 184) },
			{ (2 << shift) + 0, new Color(138, 73, 56) },
			{ (2 << shift) + 1, new Color(174, 79, 55) },
			{ (2 << shift) + 2, new Color(202, 107, 83) },
			{ (2 << shift) + 3, new Color(220, 162, 86) },
			{ (2 << shift) + 4, new Color(239, 203, 70) },
			{ (3 << shift) + 0, new Color(122, 90, 144) },
			{ (3 << shift) + 1, new Color(169, 106, 165) },
			{ (3 << shift) + 2, new Color(55, 109, 85) },
			{ (3 << shift) + 3, new Color(71, 141, 110) },
			{ (3 << shift) + 4, new Color(225, 184, 95) },
			{ (4 << shift) + 0, new Color(130, 85, 114) },
			{ (4 << shift) + 1, new Color(179, 97, 121) },
			{ (4 << shift) + 2, new Color(214, 118, 147) },
			{ (4 << shift) + 3, new Color(56, 96, 61) },
			{ (4 << shift) + 4, new Color(82, 171, 159) },
			{ (5 << shift) + 0, new Color(94, 87, 59) },
			{ (5 << shift) + 1, new Color(116, 105, 62) },
			{ (5 << shift) + 2, new Color(140, 134, 59) },
			{ (5 << shift) + 3, new Color(176, 167, 64) },
			{ (5 << shift) + 4, new Color(192, 201, 85) },
			{ (6 << shift) + 0, new Color(154, 114, 45) },
			{ (6 << shift) + 1, new Color(182, 142, 47) },
			{ (6 << shift) + 2, new Color(210, 159, 70) },
			{ (6 << shift) + 3, new Color(122, 96, 51) },
			{ (6 << shift) + 4, new Color(64, 51, 41) }
		};
		private static readonly Dictionary<int, Color> palette_contrast = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(43, 43, 43) },
			{ (0 << shift) + 1, new Color(115, 115, 115) },
			{ (0 << shift) + 2, new Color(195, 195, 195) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(8, 8, 8) },
			{ (1 << shift) + 0, new Color(32, 37, 47) },
			{ (1 << shift) + 1, new Color(49, 58, 77) },
			{ (1 << shift) + 2, new Color(62, 118, 136) },
			{ (1 << shift) + 3, new Color(95, 157, 209) },
			{ (1 << shift) + 4, new Color(131, 200, 229) },
			{ (2 << shift) + 0, new Color(81, 30, 20) },
			{ (2 << shift) + 1, new Color(139, 40, 30) },
			{ (2 << shift) + 2, new Color(229, 83, 59) },
			{ (2 << shift) + 3, new Color(228, 153, 80) },
			{ (2 << shift) + 4, new Color(237, 226, 133) },
			{ (3 << shift) + 0, new Color(96, 57, 129) },
			{ (3 << shift) + 1, new Color(142, 94, 156) },
			{ (3 << shift) + 2, new Color(71, 89, 177) },
			{ (3 << shift) + 3, new Color(85, 122, 224) },
			{ (3 << shift) + 4, new Color(255, 189, 71) },
			{ (4 << shift) + 0, new Color(104, 46, 76) },
			{ (4 << shift) + 1, new Color(217, 57, 106) },
			{ (4 << shift) + 2, new Color(235, 145, 202) },
			{ (4 << shift) + 3, new Color(41, 72, 145) },
			{ (4 << shift) + 4, new Color(115, 171, 243) },
			{ (5 << shift) + 0, new Color(48, 56, 36) },
			{ (5 << shift) + 1, new Color(75, 92, 28) },
			{ (5 << shift) + 2, new Color(92, 131, 57) },
			{ (5 << shift) + 3, new Color(165, 177, 63) },
			{ (5 << shift) + 4, new Color(187, 214, 78) },
			{ (6 << shift) + 0, new Color(80, 63, 36) },
			{ (6 << shift) + 1, new Color(144, 103, 62) },
			{ (6 << shift) + 2, new Color(194, 158, 70) },
			{ (6 << shift) + 3, new Color(56, 48, 36) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_crystal = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(79, 56, 75) },
			{ (0 << shift) + 1, new Color(140, 90, 130) },
			{ (0 << shift) + 2, new Color(210, 167, 203) },
			{ (0 << shift) + 3, new Color(250, 232, 253) },
			{ (0 << shift) + 4, new Color(31, 19, 32) },
			{ (1 << shift) + 0, new Color(72, 40, 68) },
			{ (1 << shift) + 1, new Color(77, 55, 109) },
			{ (1 << shift) + 2, new Color(63, 92, 174) },
			{ (1 << shift) + 3, new Color(94, 144, 209) },
			{ (1 << shift) + 4, new Color(131, 200, 229) },
			{ (2 << shift) + 0, new Color(90, 42, 66) },
			{ (2 << shift) + 1, new Color(133, 50, 81) },
			{ (2 << shift) + 2, new Color(200, 86, 110) },
			{ (2 << shift) + 3, new Color(225, 137, 137) },
			{ (2 << shift) + 4, new Color(239, 195, 164) },
			{ (3 << shift) + 0, new Color(102, 64, 120) },
			{ (3 << shift) + 1, new Color(159, 89, 166) },
			{ (3 << shift) + 2, new Color(85, 86, 157) },
			{ (3 << shift) + 3, new Color(116, 128, 203) },
			{ (3 << shift) + 4, new Color(232, 166, 141) },
			{ (4 << shift) + 0, new Color(108, 56, 98) },
			{ (4 << shift) + 1, new Color(201, 82, 151) },
			{ (4 << shift) + 2, new Color(245, 142, 216) },
			{ (4 << shift) + 3, new Color(84, 72, 145) },
			{ (4 << shift) + 4, new Color(134, 169, 223) },
			{ (5 << shift) + 0, new Color(42, 71, 56) },
			{ (5 << shift) + 1, new Color(58, 106, 71) },
			{ (5 << shift) + 2, new Color(90, 153, 99) },
			{ (5 << shift) + 3, new Color(138, 187, 129) },
			{ (5 << shift) + 4, new Color(178, 223, 156) },
			{ (6 << shift) + 0, new Color(128, 66, 71) },
			{ (6 << shift) + 1, new Color(181, 95, 74) },
			{ (6 << shift) + 2, new Color(205, 139, 91) },
			{ (6 << shift) + 3, new Color(71, 42, 52) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_default = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(36, 36, 36) },
			{ (0 << shift) + 1, new Color(115, 115, 115) },
			{ (0 << shift) + 2, new Color(195, 195, 195) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(8, 8, 8) },
			{ (1 << shift) + 0, new Color(21, 24, 31) },
			{ (1 << shift) + 1, new Color(41, 49, 65) },
			{ (1 << shift) + 2, new Color(62, 118, 136) },
			{ (1 << shift) + 3, new Color(95, 157, 209) },
			{ (1 << shift) + 4, new Color(131, 200, 229) },
			{ (2 << shift) + 0, new Color(66, 25, 16) },
			{ (2 << shift) + 1, new Color(130, 38, 28) },
			{ (2 << shift) + 2, new Color(229, 83, 59) },
			{ (2 << shift) + 3, new Color(228, 153, 80) },
			{ (2 << shift) + 4, new Color(237, 226, 133) },
			{ (3 << shift) + 0, new Color(96, 57, 129) },
			{ (3 << shift) + 1, new Color(142, 94, 156) },
			{ (3 << shift) + 2, new Color(71, 89, 177) },
			{ (3 << shift) + 3, new Color(85, 122, 224) },
			{ (3 << shift) + 4, new Color(255, 189, 71) },
			{ (4 << shift) + 0, new Color(104, 46, 76) },
			{ (4 << shift) + 1, new Color(217, 57, 106) },
			{ (4 << shift) + 2, new Color(235, 145, 202) },
			{ (4 << shift) + 3, new Color(41, 72, 145) },
			{ (4 << shift) + 4, new Color(115, 171, 243) },
			{ (5 << shift) + 0, new Color(48, 56, 36) },
			{ (5 << shift) + 1, new Color(75, 92, 28) },
			{ (5 << shift) + 2, new Color(92, 131, 57) },
			{ (5 << shift) + 3, new Color(165, 177, 63) },
			{ (5 << shift) + 4, new Color(182, 211, 64) },
			{ (6 << shift) + 0, new Color(80, 63, 36) },
			{ (6 << shift) + 1, new Color(144, 103, 62) },
			{ (6 << shift) + 2, new Color(194, 158, 70) },
			{ (6 << shift) + 3, new Color(54, 46, 34) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_factory = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(60, 49, 49) },
			{ (0 << shift) + 1, new Color(124, 105, 105) },
			{ (0 << shift) + 2, new Color(201, 188, 188) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(21, 13, 13) },
			{ (1 << shift) + 0, new Color(40, 26, 26) },
			{ (1 << shift) + 1, new Color(68, 45, 45) },
			{ (1 << shift) + 2, new Color(86, 123, 159) },
			{ (1 << shift) + 3, new Color(109, 154, 186) },
			{ (1 << shift) + 4, new Color(154, 192, 208) },
			{ (2 << shift) + 0, new Color(61, 36, 36) },
			{ (2 << shift) + 1, new Color(122, 61, 51) },
			{ (2 << shift) + 2, new Color(200, 109, 86) },
			{ (2 << shift) + 3, new Color(225, 164, 110) },
			{ (2 << shift) + 4, new Color(232, 225, 161) },
			{ (3 << shift) + 0, new Color(93, 62, 116) },
			{ (3 << shift) + 1, new Color(138, 86, 143) },
			{ (3 << shift) + 2, new Color(86, 101, 138) },
			{ (3 << shift) + 3, new Color(117, 132, 185) },
			{ (3 << shift) + 4, new Color(237, 198, 129) },
			{ (4 << shift) + 0, new Color(105, 63, 89) },
			{ (4 << shift) + 1, new Color(202, 104, 140) },
			{ (4 << shift) + 2, new Color(235, 145, 202) },
			{ (4 << shift) + 3, new Color(85, 75, 104) },
			{ (4 << shift) + 4, new Color(133, 160, 211) },
			{ (5 << shift) + 0, new Color(49, 56, 41) },
			{ (5 << shift) + 1, new Color(82, 96, 57) },
			{ (5 << shift) + 2, new Color(120, 132, 72) },
			{ (5 << shift) + 3, new Color(164, 169, 93) },
			{ (5 << shift) + 4, new Color(198, 203, 125) },
			{ (6 << shift) + 0, new Color(78, 64, 55) },
			{ (6 << shift) + 1, new Color(133, 108, 76) },
			{ (6 << shift) + 2, new Color(201, 162, 94) },
			{ (6 << shift) + 3, new Color(45, 35, 32) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_garden = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(55, 72, 55) },
			{ (0 << shift) + 1, new Color(78, 116, 80) },
			{ (0 << shift) + 2, new Color(130, 171, 132) },
			{ (0 << shift) + 3, new Color(227, 236, 227) },
			{ (0 << shift) + 4, new Color(18, 23, 8) },
			{ (1 << shift) + 0, new Color(47, 62, 35) },
			{ (1 << shift) + 1, new Color(66, 81, 57) },
			{ (1 << shift) + 2, new Color(53, 143, 100) },
			{ (1 << shift) + 3, new Color(73, 202, 164) },
			{ (1 << shift) + 4, new Color(133, 229, 207) },
			{ (2 << shift) + 0, new Color(111, 54, 32) },
			{ (2 << shift) + 1, new Color(169, 72, 34) },
			{ (2 << shift) + 2, new Color(224, 110, 82) },
			{ (2 << shift) + 3, new Color(228, 153, 80) },
			{ (2 << shift) + 4, new Color(237, 226, 133) },
			{ (3 << shift) + 0, new Color(91, 72, 151) },
			{ (3 << shift) + 1, new Color(161, 122, 184) },
			{ (3 << shift) + 2, new Color(76, 103, 153) },
			{ (3 << shift) + 3, new Color(78, 126, 177) },
			{ (3 << shift) + 4, new Color(241, 189, 110) },
			{ (4 << shift) + 0, new Color(111, 61, 141) },
			{ (4 << shift) + 1, new Color(202, 78, 194) },
			{ (4 << shift) + 2, new Color(222, 147, 235) },
			{ (4 << shift) + 3, new Color(56, 94, 113) },
			{ (4 << shift) + 4, new Color(91, 157, 199) },
			{ (5 << shift) + 0, new Color(71, 92, 40) },
			{ (5 << shift) + 1, new Color(96, 121, 27) },
			{ (5 << shift) + 2, new Color(137, 164, 30) },
			{ (5 << shift) + 3, new Color(189, 202, 22) },
			{ (5 << shift) + 4, new Color(215, 229, 41) },
			{ (6 << shift) + 0, new Color(89, 92, 51) },
			{ (6 << shift) + 1, new Color(124, 129, 54) },
			{ (6 << shift) + 2, new Color(184, 182, 61) },
			{ (6 << shift) + 3, new Color(66, 67, 40) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_marshmallow = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(36, 36, 36) },
			{ (0 << shift) + 1, new Color(211, 130, 155) },
			{ (0 << shift) + 2, new Color(195, 195, 195) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(113, 35, 86) },
			{ (1 << shift) + 0, new Color(150, 49, 93) },
			{ (1 << shift) + 1, new Color(212, 84, 122) },
			{ (1 << shift) + 2, new Color(142, 112, 219) },
			{ (1 << shift) + 3, new Color(141, 147, 226) },
			{ (1 << shift) + 4, new Color(154, 175, 229) },
			{ (2 << shift) + 0, new Color(146, 47, 105) },
			{ (2 << shift) + 1, new Color(193, 62, 139) },
			{ (2 << shift) + 2, new Color(239, 94, 138) },
			{ (2 << shift) + 3, new Color(249, 139, 128) },
			{ (2 << shift) + 4, new Color(231, 222, 141) },
			{ (3 << shift) + 0, new Color(148, 87, 183) },
			{ (3 << shift) + 1, new Color(181, 109, 223) },
			{ (3 << shift) + 2, new Color(115, 100, 213) },
			{ (3 << shift) + 3, new Color(131, 137, 229) },
			{ (3 << shift) + 4, new Color(249, 176, 137) },
			{ (4 << shift) + 0, new Color(173, 83, 168) },
			{ (4 << shift) + 1, new Color(226, 85, 198) },
			{ (4 << shift) + 2, new Color(245, 150, 215) },
			{ (4 << shift) + 3, new Color(111, 83, 151) },
			{ (4 << shift) + 4, new Color(151, 174, 233) },
			{ (5 << shift) + 0, new Color(69, 110, 95) },
			{ (5 << shift) + 1, new Color(68, 141, 72) },
			{ (5 << shift) + 2, new Color(85, 166, 78) },
			{ (5 << shift) + 3, new Color(119, 194, 97) },
			{ (5 << shift) + 4, new Color(164, 225, 120) },
			{ (6 << shift) + 0, new Color(184, 81, 90) },
			{ (6 << shift) + 1, new Color(212, 128, 115) },
			{ (6 << shift) + 2, new Color(236, 142, 91) },
			{ (6 << shift) + 3, new Color(152, 82, 108) },
			{ (6 << shift) + 4, new Color(103, 32, 79) }
		};
		private static readonly Dictionary<int, Color> palette_mono = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(36, 36, 36) },
			{ (0 << shift) + 1, new Color(115, 115, 115) },
			{ (0 << shift) + 2, new Color(195, 195, 195) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(8, 8, 8) },
			{ (1 << shift) + 0, new Color(25, 25, 25) },
			{ (1 << shift) + 1, new Color(51, 51, 51) },
			{ (1 << shift) + 2, new Color(115, 115, 115) },
			{ (1 << shift) + 3, new Color(195, 195, 195) },
			{ (1 << shift) + 4, new Color(255, 255, 255) },
			{ (2 << shift) + 0, new Color(42, 42, 42) },
			{ (2 << shift) + 1, new Color(61, 61, 61) },
			{ (2 << shift) + 2, new Color(115, 115, 115) },
			{ (2 << shift) + 3, new Color(184, 184, 184) },
			{ (2 << shift) + 4, new Color(255, 255, 255) },
			{ (3 << shift) + 0, new Color(78, 78, 78) },
			{ (3 << shift) + 1, new Color(115, 115, 115) },
			{ (3 << shift) + 2, new Color(97, 97, 97) },
			{ (3 << shift) + 3, new Color(136, 136, 136) },
			{ (3 << shift) + 4, new Color(214, 214, 214) },
			{ (4 << shift) + 0, new Color(61, 61, 61) },
			{ (4 << shift) + 1, new Color(136, 136, 136) },
			{ (4 << shift) + 2, new Color(233, 233, 233) },
			{ (4 << shift) + 3, new Color(68, 68, 68) },
			{ (4 << shift) + 4, new Color(165, 165, 165) },
			{ (5 << shift) + 0, new Color(42, 42, 42) },
			{ (5 << shift) + 1, new Color(106, 106, 106) },
			{ (5 << shift) + 2, new Color(178, 178, 178) },
			{ (5 << shift) + 3, new Color(218, 218, 218) },
			{ (5 << shift) + 4, new Color(255, 255, 255) },
			{ (6 << shift) + 0, new Color(42, 42, 42) },
			{ (6 << shift) + 1, new Color(115, 115, 115) },
			{ (6 << shift) + 2, new Color(195, 195, 195) },
			{ (6 << shift) + 3, new Color(25, 25, 25) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_mountain = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(64, 77, 88) },
			{ (0 << shift) + 1, new Color(100, 118, 135) },
			{ (0 << shift) + 2, new Color(164, 176, 188) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(30, 38, 46) },
			{ (1 << shift) + 0, new Color(41, 59, 76) },
			{ (1 << shift) + 1, new Color(62, 87, 111) },
			{ (1 << shift) + 2, new Color(49, 141, 165) },
			{ (1 << shift) + 3, new Color(70, 186, 206) },
			{ (1 << shift) + 4, new Color(138, 229, 215) },
			{ (2 << shift) + 0, new Color(97, 56, 59) },
			{ (2 << shift) + 1, new Color(139, 76, 60) },
			{ (2 << shift) + 2, new Color(181, 102, 59) },
			{ (2 << shift) + 3, new Color(223, 154, 67) },
			{ (2 << shift) + 4, new Color(227, 211, 61) },
			{ (3 << shift) + 0, new Color(93, 68, 156) },
			{ (3 << shift) + 1, new Color(142, 94, 156) },
			{ (3 << shift) + 2, new Color(60, 118, 161) },
			{ (3 << shift) + 3, new Color(80, 134, 189) },
			{ (3 << shift) + 4, new Color(233, 187, 63) },
			{ (4 << shift) + 0, new Color(112, 68, 156) },
			{ (4 << shift) + 1, new Color(172, 82, 188) },
			{ (4 << shift) + 2, new Color(205, 141, 231) },
			{ (4 << shift) + 3, new Color(60, 96, 150) },
			{ (4 << shift) + 4, new Color(90, 153, 201) },
			{ (5 << shift) + 0, new Color(41, 98, 65) },
			{ (5 << shift) + 1, new Color(48, 136, 59) },
			{ (5 << shift) + 2, new Color(62, 177, 53) },
			{ (5 << shift) + 3, new Color(104, 212, 58) },
			{ (5 << shift) + 4, new Color(133, 231, 125) },
			{ (6 << shift) + 0, new Color(66, 87, 51) },
			{ (6 << shift) + 1, new Color(105, 125, 69) },
			{ (6 << shift) + 2, new Color(151, 164, 70) },
			{ (6 << shift) + 3, new Color(38, 64, 47) },
			{ (6 << shift) + 4, new Color(19, 31, 23) }
		};
		private static readonly Dictionary<int, Color> palette_ocean = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(36, 36, 36) },
			{ (0 << shift) + 1, new Color(81, 111, 148) },
			{ (0 << shift) + 2, new Color(195, 195, 195) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(15, 21, 41) },
			{ (1 << shift) + 0, new Color(36, 44, 71) },
			{ (1 << shift) + 1, new Color(46, 66, 92) },
			{ (1 << shift) + 2, new Color(41, 60, 127) },
			{ (1 << shift) + 3, new Color(52, 97, 179) },
			{ (1 << shift) + 4, new Color(97, 147, 209) },
			{ (2 << shift) + 0, new Color(88, 40, 74) },
			{ (2 << shift) + 1, new Color(137, 61, 94) },
			{ (2 << shift) + 2, new Color(196, 90, 117) },
			{ (2 << shift) + 3, new Color(227, 142, 89) },
			{ (2 << shift) + 4, new Color(224, 240, 157) },
			{ (3 << shift) + 0, new Color(108, 52, 141) },
			{ (3 << shift) + 1, new Color(159, 99, 205) },
			{ (3 << shift) + 2, new Color(41, 68, 161) },
			{ (3 << shift) + 3, new Color(27, 97, 167) },
			{ (3 << shift) + 4, new Color(247, 197, 108) },
			{ (4 << shift) + 0, new Color(128, 52, 141) },
			{ (4 << shift) + 1, new Color(191, 99, 217) },
			{ (4 << shift) + 2, new Color(241, 120, 242) },
			{ (4 << shift) + 3, new Color(42, 62, 131) },
			{ (4 << shift) + 4, new Color(35, 122, 187) },
			{ (5 << shift) + 0, new Color(47, 81, 52) },
			{ (5 << shift) + 1, new Color(64, 116, 60) },
			{ (5 << shift) + 2, new Color(84, 150, 64) },
			{ (5 << shift) + 3, new Color(119, 171, 63) },
			{ (5 << shift) + 4, new Color(146, 199, 89) },
			{ (6 << shift) + 0, new Color(68, 75, 58) },
			{ (6 << shift) + 1, new Color(91, 98, 70) },
			{ (6 << shift) + 2, new Color(132, 131, 77) },
			{ (6 << shift) + 3, new Color(42, 47, 39) },
			{ (6 << shift) + 4, new Color(12, 17, 34) }
		};
		private static readonly Dictionary<int, Color> palette_ruins = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(37, 42, 47) },
			{ (0 << shift) + 1, new Color(77, 80, 92) },
			{ (0 << shift) + 2, new Color(122, 127, 144) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(22, 23, 18) },
			{ (1 << shift) + 0, new Color(38, 40, 31) },
			{ (1 << shift) + 1, new Color(59, 72, 68) },
			{ (1 << shift) + 2, new Color(48, 93, 96) },
			{ (1 << shift) + 3, new Color(59, 119, 151) },
			{ (1 << shift) + 4, new Color(94, 172, 212) },
			{ (2 << shift) + 0, new Color(87, 53, 50) },
			{ (2 << shift) + 1, new Color(134, 81, 60) },
			{ (2 << shift) + 2, new Color(202, 125, 83) },
			{ (2 << shift) + 3, new Color(208, 164, 98) },
			{ (2 << shift) + 4, new Color(239, 220, 104) },
			{ (3 << shift) + 0, new Color(107, 77, 116) },
			{ (3 << shift) + 1, new Color(178, 112, 174) },
			{ (3 << shift) + 2, new Color(63, 99, 104) },
			{ (3 << shift) + 3, new Color(70, 124, 122) },
			{ (3 << shift) + 4, new Color(221, 197, 93) },
			{ (4 << shift) + 0, new Color(114, 69, 98) },
			{ (4 << shift) + 1, new Color(181, 83, 132) },
			{ (4 << shift) + 2, new Color(210, 132, 171) },
			{ (4 << shift) + 3, new Color(48, 84, 79) },
			{ (4 << shift) + 4, new Color(86, 146, 159) },
			{ (5 << shift) + 0, new Color(51, 62, 45) },
			{ (5 << shift) + 1, new Color(66, 90, 54) },
			{ (5 << shift) + 2, new Color(87, 128, 56) },
			{ (5 << shift) + 3, new Color(129, 175, 65) },
			{ (5 << shift) + 4, new Color(170, 203, 88) },
			{ (6 << shift) + 0, new Color(86, 93, 45) },
			{ (6 << shift) + 1, new Color(112, 102, 41) },
			{ (6 << shift) + 2, new Color(156, 124, 48) },
			{ (6 << shift) + 3, new Color(46, 51, 26) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_space = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(67, 52, 83) },
			{ (0 << shift) + 1, new Color(108, 77, 128) },
			{ (0 << shift) + 2, new Color(176, 126, 192) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(26, 19, 35) },
			{ (1 << shift) + 0, new Color(38, 30, 67) },
			{ (1 << shift) + 1, new Color(50, 47, 96) },
			{ (1 << shift) + 2, new Color(93, 66, 194) },
			{ (1 << shift) + 3, new Color(113, 113, 214) },
			{ (1 << shift) + 4, new Color(148, 165, 224) },
			{ (2 << shift) + 0, new Color(112, 36, 53) },
			{ (2 << shift) + 1, new Color(171, 43, 82) },
			{ (2 << shift) + 2, new Color(217, 75, 69) },
			{ (2 << shift) + 3, new Color(219, 126, 86) },
			{ (2 << shift) + 4, new Color(237, 226, 133) },
			{ (3 << shift) + 0, new Color(64, 52, 84) },
			{ (3 << shift) + 1, new Color(116, 68, 151) },
			{ (3 << shift) + 2, new Color(62, 63, 153) },
			{ (3 << shift) + 3, new Color(76, 84, 195) },
			{ (3 << shift) + 4, new Color(235, 166, 92) },
			{ (4 << shift) + 0, new Color(90, 52, 85) },
			{ (4 << shift) + 1, new Color(170, 65, 156) },
			{ (4 << shift) + 2, new Color(211, 107, 205) },
			{ (4 << shift) + 3, new Color(61, 53, 126) },
			{ (4 << shift) + 4, new Color(91, 118, 209) },
			{ (5 << shift) + 0, new Color(48, 56, 36) },
			{ (5 << shift) + 1, new Color(75, 92, 28) },
			{ (5 << shift) + 2, new Color(92, 131, 57) },
			{ (5 << shift) + 3, new Color(139, 165, 60) },
			{ (5 << shift) + 4, new Color(173, 195, 83) },
			{ (6 << shift) + 0, new Color(93, 60, 63) },
			{ (6 << shift) + 1, new Color(144, 93, 81) },
			{ (6 << shift) + 2, new Color(184, 131, 81) },
			{ (6 << shift) + 3, new Color(65, 47, 54) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_swamp = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(34, 76, 57) },
			{ (0 << shift) + 1, new Color(62, 137, 104) },
			{ (0 << shift) + 2, new Color(166, 186, 176) },
			{ (0 << shift) + 3, new Color(211, 228, 211) },
			{ (0 << shift) + 4, new Color(21, 46, 24) },
			{ (1 << shift) + 0, new Color(30, 67, 35) },
			{ (1 << shift) + 1, new Color(46, 102, 66) },
			{ (1 << shift) + 2, new Color(62, 127, 152) },
			{ (1 << shift) + 3, new Color(64, 164, 181) },
			{ (1 << shift) + 4, new Color(118, 214, 199) },
			{ (2 << shift) + 0, new Color(121, 52, 67) },
			{ (2 << shift) + 1, new Color(167, 62, 62) },
			{ (2 << shift) + 2, new Color(197, 89, 89) },
			{ (2 << shift) + 3, new Color(215, 143, 91) },
			{ (2 << shift) + 4, new Color(237, 207, 100) },
			{ (3 << shift) + 0, new Color(79, 64, 135) },
			{ (3 << shift) + 1, new Color(137, 99, 181) },
			{ (3 << shift) + 2, new Color(80, 111, 187) },
			{ (3 << shift) + 3, new Color(85, 134, 195) },
			{ (3 << shift) + 4, new Color(237, 171, 102) },
			{ (4 << shift) + 0, new Color(108, 66, 151) },
			{ (4 << shift) + 1, new Color(180, 110, 145) },
			{ (4 << shift) + 2, new Color(220, 147, 195) },
			{ (4 << shift) + 3, new Color(62, 112, 107) },
			{ (4 << shift) + 4, new Color(92, 162, 207) },
			{ (5 << shift) + 0, new Color(28, 84, 31) },
			{ (5 << shift) + 1, new Color(61, 132, 21) },
			{ (5 << shift) + 2, new Color(102, 159, 25) },
			{ (5 << shift) + 3, new Color(139, 187, 21) },
			{ (5 << shift) + 4, new Color(170, 217, 23) },
			{ (6 << shift) + 0, new Color(163, 90, 62) },
			{ (6 << shift) + 1, new Color(203, 115, 52) },
			{ (6 << shift) + 2, new Color(225, 156, 65) },
			{ (6 << shift) + 3, new Color(119, 84, 64) },
			{ (6 << shift) + 4, new Color(18, 40, 21) }
		};
		private static readonly Dictionary<int, Color> palette_test = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(36, 36, 36) },
			{ (0 << shift) + 1, new Color(115, 115, 115) },
			{ (0 << shift) + 2, new Color(195, 195, 195) },
			{ (0 << shift) + 3, new Color(255, 0, 0) },
			{ (0 << shift) + 4, new Color(8, 8, 8) },
			{ (1 << shift) + 0, new Color(25, 25, 25) },
			{ (1 << shift) + 1, new Color(51, 51, 51) },
			{ (1 << shift) + 2, new Color(255, 0, 0) },
			{ (1 << shift) + 3, new Color(195, 195, 195) },
			{ (1 << shift) + 4, new Color(255, 255, 255) },
			{ (2 << shift) + 0, new Color(42, 42, 42) },
			{ (2 << shift) + 1, new Color(255, 0, 0) },
			{ (2 << shift) + 2, new Color(255, 0, 0) },
			{ (2 << shift) + 3, new Color(195, 195, 195) },
			{ (2 << shift) + 4, new Color(255, 255, 255) },
			{ (3 << shift) + 0, new Color(61, 61, 61) },
			{ (3 << shift) + 1, new Color(255, 0, 0) },
			{ (3 << shift) + 2, new Color(97, 97, 97) },
			{ (3 << shift) + 3, new Color(136, 136, 136) },
			{ (3 << shift) + 4, new Color(216, 216, 216) },
			{ (4 << shift) + 0, new Color(61, 61, 61) },
			{ (4 << shift) + 1, new Color(255, 0, 0) },
			{ (4 << shift) + 2, new Color(255, 255, 255) },
			{ (4 << shift) + 3, new Color(42, 42, 42) },
			{ (4 << shift) + 4, new Color(195, 195, 195) },
			{ (5 << shift) + 0, new Color(42, 42, 42) },
			{ (5 << shift) + 1, new Color(255, 0, 0) },
			{ (5 << shift) + 2, new Color(255, 0, 0) },
			{ (5 << shift) + 3, new Color(216, 216, 216) },
			{ (5 << shift) + 4, new Color(255, 255, 255) },
			{ (6 << shift) + 0, new Color(42, 42, 42) },
			{ (6 << shift) + 1, new Color(115, 115, 115) },
			{ (6 << shift) + 2, new Color(255, 0, 0) },
			{ (6 << shift) + 3, new Color(25, 25, 25) },
			{ (6 << shift) + 4, new Color(11, 11, 14) }
		};
		private static readonly Dictionary<int, Color> palette_variant = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(47, 41, 58) },
			{ (0 << shift) + 1, new Color(108, 98, 130) },
			{ (0 << shift) + 2, new Color(178, 166, 206) },
			{ (0 << shift) + 3, new Color(255, 255, 255) },
			{ (0 << shift) + 4, new Color(12, 6, 13) },
			{ (1 << shift) + 0, new Color(25, 28, 40) },
			{ (1 << shift) + 1, new Color(52, 48, 85) },
			{ (1 << shift) + 2, new Color(59, 157, 133) },
			{ (1 << shift) + 3, new Color(105, 226, 223) },
			{ (1 << shift) + 4, new Color(187, 255, 243) },
			{ (2 << shift) + 0, new Color(78, 11, 32) },
			{ (2 << shift) + 1, new Color(154, 18, 71) },
			{ (2 << shift) + 2, new Color(249, 65, 128) },
			{ (2 << shift) + 3, new Color(253, 125, 125) },
			{ (2 << shift) + 4, new Color(250, 193, 153) },
			{ (3 << shift) + 0, new Color(87, 52, 112) },
			{ (3 << shift) + 1, new Color(139, 87, 183) },
			{ (3 << shift) + 2, new Color(62, 79, 155) },
			{ (3 << shift) + 3, new Color(86, 117, 185) },
			{ (3 << shift) + 4, new Color(255, 159, 126) },
			{ (4 << shift) + 0, new Color(129, 58, 129) },
			{ (4 << shift) + 1, new Color(228, 87, 198) },
			{ (4 << shift) + 2, new Color(227, 145, 239) },
			{ (4 << shift) + 3, new Color(60, 58, 126) },
			{ (4 << shift) + 4, new Color(100, 151, 197) },
			{ (5 << shift) + 0, new Color(63, 61, 37) },
			{ (5 << shift) + 1, new Color(108, 89, 23) },
			{ (5 << shift) + 2, new Color(138, 141, 51) },
			{ (5 << shift) + 3, new Color(175, 179, 65) },
			{ (5 << shift) + 4, new Color(217, 221, 96) },
			{ (6 << shift) + 0, new Color(92, 42, 34) },
			{ (6 << shift) + 1, new Color(167, 60, 58) },
			{ (6 << shift) + 2, new Color(212, 106, 76) },
			{ (6 << shift) + 3, new Color(61, 38, 35) },
			{ (6 << shift) + 4, new Color(12, 14, 16) }
		};
		private static readonly Dictionary<int, Color> palette_volcano = new Dictionary<int, Color>() { 
			{ (0 << shift) + 0, new Color(36, 36, 36) },
			{ (0 << shift) + 1, new Color(161, 95, 69) },
			{ (0 << shift) + 2, new Color(207, 181, 166) },
			{ (0 << shift) + 3, new Color(234, 210, 169) },
			{ (0 << shift) + 4, new Color(38, 29, 29) },
			{ (1 << shift) + 0, new Color(95, 65, 53) },
			{ (1 << shift) + 1, new Color(126, 78, 53) },
			{ (1 << shift) + 2, new Color(94, 146, 79) },
			{ (1 << shift) + 3, new Color(112, 191, 89) },
			{ (1 << shift) + 4, new Color(155, 229, 133) },
			{ (2 << shift) + 0, new Color(147, 39, 39) },
			{ (2 << shift) + 1, new Color(192, 58, 38) },
			{ (2 << shift) + 2, new Color(224, 100, 36) },
			{ (2 << shift) + 3, new Color(241, 150, 81) },
			{ (2 << shift) + 4, new Color(242, 209, 90) },
			{ (3 << shift) + 0, new Color(169, 90, 147) },
			{ (3 << shift) + 1, new Color(179, 134, 198) },
			{ (3 << shift) + 2, new Color(86, 124, 75) },
			{ (3 << shift) + 3, new Color(91, 157, 109) },
			{ (3 << shift) + 4, new Color(241, 182, 93) },
			{ (4 << shift) + 0, new Color(169, 76, 96) },
			{ (4 << shift) + 1, new Color(189, 124, 144) },
			{ (4 << shift) + 2, new Color(221, 167, 185) },
			{ (4 << shift) + 3, new Color(108, 108, 82) },
			{ (4 << shift) + 4, new Color(111, 185, 156) },
			{ (5 << shift) + 0, new Color(96, 71, 47) },
			{ (5 << shift) + 1, new Color(133, 104, 53) },
			{ (5 << shift) + 2, new Color(161, 127, 46) },
			{ (5 << shift) + 3, new Color(186, 164, 38) },
			{ (5 << shift) + 4, new Color(221, 196, 57) },
			{ (6 << shift) + 0, new Color(136, 67, 48) },
			{ (6 << shift) + 1, new Color(177, 96, 53) },
			{ (6 << shift) + 2, new Color(196, 116, 59) },
			{ (6 << shift) + 3, new Color(108, 60, 45) },
			{ (6 << shift) + 4, new Color(48, 37, 36) }
		};
        public static Dictionary<string, Dictionary<int, Color>> Palettes = new Dictionary<string, Dictionary<int, Color>>() {
			{ "abstract", palette_abstract },
			{ "autumn", palette_autumn },
			{ "contrast", palette_contrast },
			{ "crystal", palette_crystal },
			{ "default", palette_default },
			{ "factory", palette_factory },
			{ "garden", palette_garden },
			{ "marshmallow", palette_marshmallow },
			{ "mono", palette_mono },
			{ "mountain", palette_mountain },
			{ "ocean", palette_ocean },
			{ "ruins", palette_ruins },
			{ "space", palette_space },
			{ "swamp", palette_swamp },
			{ "test", palette_test },
			{ "variant", palette_variant },
			{ "volcano", palette_volcano },        
        };
    }
}
