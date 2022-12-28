
using Core.Utils; 
using Microsoft.Xna.Framework.Graphics; 
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Core.Content {
    public static class SheetMap {
        public static Dictionary<string, SpriteValues> GetSpriteInfo(Dictionary<string, Texture2D> sheets) {
            return new Dictionary<string, SpriteValues>() {
			{ "algae", new Wobbler("algae", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["algae"]) },
			{ "arm", new FacingOnMove(
                name: "arm", 
				up: new AnimateOnMove("arm", new Wobbler[] {
	new Wobbler("arm.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.up.2", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.up.3", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["arm"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("arm", new Wobbler[] {
	new Wobbler("arm.left.0", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.left.1", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.left.2", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["arm"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("arm", new Wobbler[] {
	new Wobbler("arm.down.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.down.1", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.down.2", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.down.3", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["arm"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("arm", new Wobbler[] {
	new Wobbler("arm.right.0", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.right.1", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.right.2", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["arm"]), 
	new Wobbler("arm.right.3", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["arm"]) }), 
				sleep_right: null
) },
			{ "arrow", new FacingOnMove(
                name: "arrow", 
				up: new AnimateOnMove("arrow", new Wobbler[] {
	new Wobbler("arrow.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["arrow"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("arrow", new Wobbler[] {
	new Wobbler("arrow.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["arrow"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("arrow", new Wobbler[] {
	new Wobbler("arrow.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["arrow"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("arrow", new Wobbler[] {
	new Wobbler("arrow.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["arrow"]) }), 
				sleep_right: null
) },
			{ "baba", new FacingOnMove(
                name: "baba", 
				up: new AnimateOnMove("baba", new Wobbler[] {
	new Wobbler("baba.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["baba"]) }), 
				sleep_up: new AnimateOnMove("baba", new Wobbler[] {
	new Wobbler("baba.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["baba"]) }), 
				left: new AnimateOnMove("baba", new Wobbler[] {
	new Wobbler("baba.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["baba"]) }), 
				sleep_left: new AnimateOnMove("baba", new Wobbler[] {
	new Wobbler("baba.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["baba"]) }), 
				down: new AnimateOnMove("baba", new Wobbler[] {
	new Wobbler("baba.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["baba"]) }), 
				sleep_down: new AnimateOnMove("baba", new Wobbler[] {
	new Wobbler("baba.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["baba"]) }), 
				right: new AnimateOnMove("baba", new Wobbler[] {
	new Wobbler("baba.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["baba"]), 
	new Wobbler("baba.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["baba"]) }), 
				sleep_right: new AnimateOnMove("baba", new Wobbler[] {
	new Wobbler("baba.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["baba"]) })
) },
			{ "badbad", new FacingOnMove(
                name: "badbad", 
				up: new AnimateOnMove("badbad", new Wobbler[] {
	new Wobbler("badbad.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["badbad"]) }), 
				sleep_up: new AnimateOnMove("badbad", new Wobbler[] {
	new Wobbler("badbad.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["badbad"]) }), 
				left: new AnimateOnMove("badbad", new Wobbler[] {
	new Wobbler("badbad.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["badbad"]) }), 
				sleep_left: new AnimateOnMove("badbad", new Wobbler[] {
	new Wobbler("badbad.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["badbad"]) }), 
				down: new AnimateOnMove("badbad", new Wobbler[] {
	new Wobbler("badbad.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["badbad"]) }), 
				sleep_down: new AnimateOnMove("badbad", new Wobbler[] {
	new Wobbler("badbad.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["badbad"]) }), 
				right: new AnimateOnMove("badbad", new Wobbler[] {
	new Wobbler("badbad.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["badbad"]), 
	new Wobbler("badbad.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["badbad"]) }), 
				sleep_right: new AnimateOnMove("badbad", new Wobbler[] {
	new Wobbler("badbad.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["badbad"]) })
) },
			{ "banana", new Wobbler("banana", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["banana"]) },
			{ "bat", new AnimateOnMove("bat", new Wobbler[] {
	new Wobbler("bat.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["bat"]), 
	new Wobbler("bat.1", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["bat"]), 
	new Wobbler("bat.2", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["bat"]), 
	new Wobbler("bat.3", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["bat"]) }) },
			{ "bed", new FacingOnMove(
                name: "bed", 
				up: new AnimateOnMove("bed", new Wobbler[] {
	new Wobbler("bed.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(26, 28), sheets["bed"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("bed", new Wobbler[] {
	new Wobbler("bed.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(26, 28), sheets["bed"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("bed", new Wobbler[] {
	new Wobbler("bed.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(26, 28), sheets["bed"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("bed", new Wobbler[] {
	new Wobbler("bed.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(26, 28), sheets["bed"]) }), 
				sleep_right: null
) },
			{ "bee", new FacingOnMove(
                name: "bee", 
				up: new AnimateOnMove("bee", new Wobbler[] {
	new Wobbler("bee.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["bee"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("bee", new Wobbler[] {
	new Wobbler("bee.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["bee"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("bee", new Wobbler[] {
	new Wobbler("bee.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["bee"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("bee", new Wobbler[] {
	new Wobbler("bee.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["bee"]) }), 
				sleep_right: null
) },
			{ "belt", new FacingOnMove(
                name: "belt", 
				up: new AnimateOnMove("belt", new Wobbler[] {
	new Wobbler("belt.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.up.2", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.up.3", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["belt"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("belt", new Wobbler[] {
	new Wobbler("belt.left.0", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.left.1", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.left.2", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["belt"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("belt", new Wobbler[] {
	new Wobbler("belt.down.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.down.1", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.down.2", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.down.3", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["belt"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("belt", new Wobbler[] {
	new Wobbler("belt.right.0", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.right.1", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.right.2", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["belt"]), 
	new Wobbler("belt.right.3", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["belt"]) }), 
				sleep_right: null
) },
			{ "bird", new FacingOnMove(
                name: "bird", 
				up: new AnimateOnMove("bird", new Wobbler[] {
	new Wobbler("bird.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["bird"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("bird", new Wobbler[] {
	new Wobbler("bird.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["bird"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("bird", new Wobbler[] {
	new Wobbler("bird.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["bird"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("bird", new Wobbler[] {
	new Wobbler("bird.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["bird"]) }), 
				sleep_right: null
) },
			{ "blob", new Joinable("blob", new Wobbler[] {
	new Wobbler("blob.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["blob"]), 
	new Wobbler("blob.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["blob"]) }) },
			{ "boat", new FacingOnMove(
                name: "boat", 
				up: new AnimateOnMove("boat", new Wobbler[] {
	new Wobbler("boat.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["boat"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("boat", new Wobbler[] {
	new Wobbler("boat.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["boat"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("boat", new Wobbler[] {
	new Wobbler("boat.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["boat"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("boat", new Wobbler[] {
	new Wobbler("boat.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["boat"]) }), 
				sleep_right: null
) },
			{ "boba", new Wobbler("boba", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["boba"]) },
			{ "bog", new Joinable("bog", new Wobbler[] {
	new Wobbler("bog.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["bog"]), 
	new Wobbler("bog.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["bog"]) }) },
			{ "bolt", new FacingOnMove(
                name: "bolt", 
				up: new AnimateOnMove("bolt", new Wobbler[] {
	new Wobbler("bolt.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["bolt"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("bolt", new Wobbler[] {
	new Wobbler("bolt.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["bolt"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("bolt", new Wobbler[] {
	new Wobbler("bolt.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["bolt"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("bolt", new Wobbler[] {
	new Wobbler("bolt.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["bolt"]) }), 
				sleep_right: null
) },
			{ "bomb", new Wobbler("bomb", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["bomb"]) },
			{ "book", new Wobbler("book", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["book"]) },
			{ "bottle", new Wobbler("bottle", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["bottle"]) },
			{ "box", new Wobbler("box", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["box"]) },
			{ "brick", new Joinable("brick", new Wobbler[] {
	new Wobbler("brick.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["brick"]), 
	new Wobbler("brick.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["brick"]) }) },
			{ "bubble", new AnimateOnMove("bubble", new Wobbler[] {
	new Wobbler("bubble.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["bubble"]), 
	new Wobbler("bubble.1", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["bubble"]), 
	new Wobbler("bubble.2", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["bubble"]), 
	new Wobbler("bubble.3", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["bubble"]) }) },
			{ "bucket", new Wobbler("bucket", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["bucket"]) },
			{ "bug", new FacingOnMove(
                name: "bug", 
				up: new AnimateOnMove("bug", new Wobbler[] {
	new Wobbler("bug.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["bug"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("bug", new Wobbler[] {
	new Wobbler("bug.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["bug"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("bug", new Wobbler[] {
	new Wobbler("bug.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["bug"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("bug", new Wobbler[] {
	new Wobbler("bug.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["bug"]) }), 
				sleep_right: null
) },
			{ "bunny", new FacingOnMove(
                name: "bunny", 
				up: new AnimateOnMove("bunny", new Wobbler[] {
	new Wobbler("bunny.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["bunny"]) }), 
				sleep_up: new AnimateOnMove("bunny", new Wobbler[] {
	new Wobbler("bunny.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["bunny"]) }), 
				left: new AnimateOnMove("bunny", new Wobbler[] {
	new Wobbler("bunny.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["bunny"]) }), 
				sleep_left: new AnimateOnMove("bunny", new Wobbler[] {
	new Wobbler("bunny.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["bunny"]) }), 
				down: new AnimateOnMove("bunny", new Wobbler[] {
	new Wobbler("bunny.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["bunny"]) }), 
				sleep_down: new AnimateOnMove("bunny", new Wobbler[] {
	new Wobbler("bunny.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["bunny"]) }), 
				right: new AnimateOnMove("bunny", new Wobbler[] {
	new Wobbler("bunny.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["bunny"]), 
	new Wobbler("bunny.right.4", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["bunny"]) }), 
				sleep_right: new AnimateOnMove("bunny", new Wobbler[] {
	new Wobbler("bunny.sleep_right.0", new[] { new Point(4, 7), new Point(5, 7), new Point(6, 7) }, new Point(24, 24), sheets["bunny"]) })
) },
			{ "burger", new Wobbler("burger", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["burger"]) },
			{ "cake", new Wobbler("cake", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["cake"]) },
			{ "car", new FacingOnMove(
                name: "car", 
				up: new AnimateOnMove("car", new Wobbler[] {
	new Wobbler("car.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["car"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("car", new Wobbler[] {
	new Wobbler("car.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["car"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("car", new Wobbler[] {
	new Wobbler("car.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["car"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("car", new Wobbler[] {
	new Wobbler("car.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["car"]) }), 
				sleep_right: null
) },
			{ "cart", new FacingOnMove(
                name: "cart", 
				up: new AnimateOnMove("cart", new Wobbler[] {
	new Wobbler("cart.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["cart"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("cart", new Wobbler[] {
	new Wobbler("cart.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["cart"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("cart", new Wobbler[] {
	new Wobbler("cart.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["cart"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("cart", new Wobbler[] {
	new Wobbler("cart.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["cart"]) }), 
				sleep_right: null
) },
			{ "cash", new Wobbler("cash", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["cash"]) },
			{ "cat", new FacingOnMove(
                name: "cat", 
				up: new AnimateOnMove("cat", new Wobbler[] {
	new Wobbler("cat.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(26, 24), sheets["cat"]) }), 
				sleep_up: new AnimateOnMove("cat", new Wobbler[] {
	new Wobbler("cat.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(26, 24), sheets["cat"]) }), 
				left: new AnimateOnMove("cat", new Wobbler[] {
	new Wobbler("cat.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(26, 24), sheets["cat"]) }), 
				sleep_left: new AnimateOnMove("cat", new Wobbler[] {
	new Wobbler("cat.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(26, 24), sheets["cat"]) }), 
				down: new AnimateOnMove("cat", new Wobbler[] {
	new Wobbler("cat.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(26, 24), sheets["cat"]) }), 
				sleep_down: new AnimateOnMove("cat", new Wobbler[] {
	new Wobbler("cat.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(26, 24), sheets["cat"]) }), 
				right: new AnimateOnMove("cat", new Wobbler[] {
	new Wobbler("cat.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(26, 24), sheets["cat"]), 
	new Wobbler("cat.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(26, 24), sheets["cat"]) }), 
				sleep_right: new AnimateOnMove("cat", new Wobbler[] {
	new Wobbler("cat.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(26, 24), sheets["cat"]) })
) },
			{ "chair", new FacingOnMove(
                name: "chair", 
				up: new AnimateOnMove("chair", new Wobbler[] {
	new Wobbler("chair.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["chair"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("chair", new Wobbler[] {
	new Wobbler("chair.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["chair"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("chair", new Wobbler[] {
	new Wobbler("chair.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["chair"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("chair", new Wobbler[] {
	new Wobbler("chair.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["chair"]) }), 
				sleep_right: null
) },
			{ "cheese", new Wobbler("cheese", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["cheese"]) },
			{ "circle", new Wobbler("circle", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["circle"]) },
			{ "cliff", new Joinable("cliff", new Wobbler[] {
	new Wobbler("cliff.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["cliff"]), 
	new Wobbler("cliff.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["cliff"]) }) },
			{ "clock", new AnimateOnMove("clock", new Wobbler[] {
	new Wobbler("clock.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["clock"]), 
	new Wobbler("clock.1", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["clock"]), 
	new Wobbler("clock.2", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["clock"]), 
	new Wobbler("clock.3", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["clock"]) }) },
			{ "cloud", new Joinable("cloud", new Wobbler[] {
	new Wobbler("cloud.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["cloud"]), 
	new Wobbler("cloud.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["cloud"]) }) },
			{ "cog", new AnimateOnMove("cog", new Wobbler[] {
	new Wobbler("cog.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["cog"]), 
	new Wobbler("cog.1", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["cog"]), 
	new Wobbler("cog.2", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["cog"]), 
	new Wobbler("cog.3", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["cog"]) }) },
			{ "crab", new FacingOnMove(
                name: "crab", 
				up: new AnimateOnMove("crab", new Wobbler[] {
	new Wobbler("crab.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["crab"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("crab", new Wobbler[] {
	new Wobbler("crab.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["crab"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("crab", new Wobbler[] {
	new Wobbler("crab.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["crab"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("crab", new Wobbler[] {
	new Wobbler("crab.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["crab"]) }), 
				sleep_right: null
) },
			{ "crystal", new FacingOnMove(
                name: "crystal", 
				up: new AnimateOnMove("crystal", new Wobbler[] {
	new Wobbler("crystal.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["crystal"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("crystal", new Wobbler[] {
	new Wobbler("crystal.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["crystal"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("crystal", new Wobbler[] {
	new Wobbler("crystal.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["crystal"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("crystal", new Wobbler[] {
	new Wobbler("crystal.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["crystal"]) }), 
				sleep_right: null
) },
			{ "cup", new Wobbler("cup", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["cup"]) },
			{ "cursor", new Wobbler("cursor", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(32, 32), sheets["cursor"]) },
			{ "dog", new FacingOnMove(
                name: "dog", 
				up: new AnimateOnMove("dog", new Wobbler[] {
	new Wobbler("dog.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(26, 24), sheets["dog"]) }), 
				sleep_up: new AnimateOnMove("dog", new Wobbler[] {
	new Wobbler("dog.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(26, 24), sheets["dog"]) }), 
				left: new AnimateOnMove("dog", new Wobbler[] {
	new Wobbler("dog.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(26, 24), sheets["dog"]) }), 
				sleep_left: new AnimateOnMove("dog", new Wobbler[] {
	new Wobbler("dog.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(26, 24), sheets["dog"]) }), 
				down: new AnimateOnMove("dog", new Wobbler[] {
	new Wobbler("dog.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(26, 24), sheets["dog"]) }), 
				sleep_down: new AnimateOnMove("dog", new Wobbler[] {
	new Wobbler("dog.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(26, 24), sheets["dog"]) }), 
				right: new AnimateOnMove("dog", new Wobbler[] {
	new Wobbler("dog.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(26, 24), sheets["dog"]), 
	new Wobbler("dog.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(26, 24), sheets["dog"]) }), 
				sleep_right: new AnimateOnMove("dog", new Wobbler[] {
	new Wobbler("dog.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(26, 24), sheets["dog"]) })
) },
			{ "donut", new Wobbler("donut", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["donut"]) },
			{ "door", new Wobbler("door", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["door"]) },
			{ "dot", new Wobbler("dot", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["dot"]) },
			{ "drink", new Wobbler("drink", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["drink"]) },
			{ "drum", new Wobbler("drum", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["drum"]) },
			{ "dust", new Wobbler("dust", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["dust"]) },
			{ "ear", new FacingOnMove(
                name: "ear", 
				up: new AnimateOnMove("ear", new Wobbler[] {
	new Wobbler("ear.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["ear"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("ear", new Wobbler[] {
	new Wobbler("ear.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["ear"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("ear", new Wobbler[] {
	new Wobbler("ear.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["ear"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("ear", new Wobbler[] {
	new Wobbler("ear.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["ear"]) }), 
				sleep_right: null
) },
			{ "egg", new FacingOnMove(
                name: "egg", 
				up: new AnimateOnMove("egg", new Wobbler[] {
	new Wobbler("egg.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["egg"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("egg", new Wobbler[] {
	new Wobbler("egg.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["egg"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("egg", new Wobbler[] {
	new Wobbler("egg.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["egg"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("egg", new Wobbler[] {
	new Wobbler("egg.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["egg"]) }), 
				sleep_right: null
) },
			{ "eye", new FacingOnMove(
                name: "eye", 
				up: new AnimateOnMove("eye", new Wobbler[] {
	new Wobbler("eye.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["eye"]) }), 
				sleep_up: new AnimateOnMove("eye", new Wobbler[] {
	new Wobbler("eye.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["eye"]) }), 
				left: new AnimateOnMove("eye", new Wobbler[] {
	new Wobbler("eye.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["eye"]) }), 
				sleep_left: new AnimateOnMove("eye", new Wobbler[] {
	new Wobbler("eye.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["eye"]) }), 
				down: new AnimateOnMove("eye", new Wobbler[] {
	new Wobbler("eye.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["eye"]) }), 
				sleep_down: new AnimateOnMove("eye", new Wobbler[] {
	new Wobbler("eye.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["eye"]) }), 
				right: new AnimateOnMove("eye", new Wobbler[] {
	new Wobbler("eye.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["eye"]), 
	new Wobbler("eye.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["eye"]) }), 
				sleep_right: new AnimateOnMove("eye", new Wobbler[] {
	new Wobbler("eye.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["eye"]) })
) },
			{ "fence", new Joinable("fence", new Wobbler[] {
	new Wobbler("fence.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["fence"]), 
	new Wobbler("fence.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["fence"]) }) },
			{ "fire", new Wobbler("fire", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["fire"]) },
			{ "fish", new FacingOnMove(
                name: "fish", 
				up: new AnimateOnMove("fish", new Wobbler[] {
	new Wobbler("fish.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["fish"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("fish", new Wobbler[] {
	new Wobbler("fish.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["fish"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("fish", new Wobbler[] {
	new Wobbler("fish.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["fish"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("fish", new Wobbler[] {
	new Wobbler("fish.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["fish"]) }), 
				sleep_right: null
) },
			{ "flag", new Wobbler("flag", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["flag"]) },
			{ "flower", new Wobbler("flower", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["flower"]) },
			{ "fofo", new FacingOnMove(
                name: "fofo", 
				up: new AnimateOnMove("fofo", new Wobbler[] {
	new Wobbler("fofo.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["fofo"]) }), 
				sleep_up: new AnimateOnMove("fofo", new Wobbler[] {
	new Wobbler("fofo.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["fofo"]) }), 
				left: new AnimateOnMove("fofo", new Wobbler[] {
	new Wobbler("fofo.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["fofo"]) }), 
				sleep_left: new AnimateOnMove("fofo", new Wobbler[] {
	new Wobbler("fofo.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["fofo"]) }), 
				down: new AnimateOnMove("fofo", new Wobbler[] {
	new Wobbler("fofo.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["fofo"]) }), 
				sleep_down: new AnimateOnMove("fofo", new Wobbler[] {
	new Wobbler("fofo.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["fofo"]) }), 
				right: new AnimateOnMove("fofo", new Wobbler[] {
	new Wobbler("fofo.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["fofo"]), 
	new Wobbler("fofo.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["fofo"]) }), 
				sleep_right: new AnimateOnMove("fofo", new Wobbler[] {
	new Wobbler("fofo.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["fofo"]) })
) },
			{ "foliage", new Wobbler("foliage", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["foliage"]) },
			{ "foot", new FacingOnMove(
                name: "foot", 
				up: new AnimateOnMove("foot", new Wobbler[] {
	new Wobbler("foot.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["foot"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("foot", new Wobbler[] {
	new Wobbler("foot.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["foot"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("foot", new Wobbler[] {
	new Wobbler("foot.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["foot"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("foot", new Wobbler[] {
	new Wobbler("foot.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["foot"]) }), 
				sleep_right: null
) },
			{ "fort", new Joinable("fort", new Wobbler[] {
	new Wobbler("fort.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["fort"]), 
	new Wobbler("fort.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["fort"]) }) },
			{ "frog", new FacingOnMove(
                name: "frog", 
				up: new AnimateOnMove("frog", new Wobbler[] {
	new Wobbler("frog.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["frog"]) }), 
				sleep_up: new AnimateOnMove("frog", new Wobbler[] {
	new Wobbler("frog.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["frog"]) }), 
				left: new AnimateOnMove("frog", new Wobbler[] {
	new Wobbler("frog.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["frog"]) }), 
				sleep_left: new AnimateOnMove("frog", new Wobbler[] {
	new Wobbler("frog.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["frog"]) }), 
				down: new AnimateOnMove("frog", new Wobbler[] {
	new Wobbler("frog.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["frog"]) }), 
				sleep_down: new AnimateOnMove("frog", new Wobbler[] {
	new Wobbler("frog.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["frog"]) }), 
				right: new AnimateOnMove("frog", new Wobbler[] {
	new Wobbler("frog.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["frog"]), 
	new Wobbler("frog.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["frog"]) }), 
				sleep_right: new AnimateOnMove("frog", new Wobbler[] {
	new Wobbler("frog.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["frog"]) })
) },
			{ "fruit", new Wobbler("fruit", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["fruit"]) },
			{ "fungi", new Wobbler("fungi", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["fungi"]) },
			{ "fungus", new Wobbler("fungus", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["fungus"]) },
			{ "gate", new Wobbler("gate", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["gate"]) },
			{ "gem", new Wobbler("gem", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["gem"]) },
			{ "ghost", new FacingOnMove(
                name: "ghost", 
				up: new AnimateOnMove("ghost", new Wobbler[] {
	new Wobbler("ghost.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["ghost"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("ghost", new Wobbler[] {
	new Wobbler("ghost.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["ghost"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("ghost", new Wobbler[] {
	new Wobbler("ghost.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["ghost"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("ghost", new Wobbler[] {
	new Wobbler("ghost.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["ghost"]) }), 
				sleep_right: null
) },
			{ "grass", new Joinable("grass", new Wobbler[] {
	new Wobbler("grass.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["grass"]), 
	new Wobbler("grass.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["grass"]) }) },
			{ "guitar", new Wobbler("guitar", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["guitar"]) },
			{ "hand", new FacingOnMove(
                name: "hand", 
				up: new AnimateOnMove("hand", new Wobbler[] {
	new Wobbler("hand.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["hand"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("hand", new Wobbler[] {
	new Wobbler("hand.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["hand"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("hand", new Wobbler[] {
	new Wobbler("hand.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["hand"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("hand", new Wobbler[] {
	new Wobbler("hand.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["hand"]) }), 
				sleep_right: null
) },
			{ "hedge", new Joinable("hedge", new Wobbler[] {
	new Wobbler("hedge.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["hedge"]), 
	new Wobbler("hedge.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["hedge"]) }) },
			{ "hihat", new Wobbler("hihat", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["hihat"]) },
			{ "house", new Wobbler("house", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["house"]) },
			{ "husk", new Wobbler("husk", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["husk"]) },
			{ "husks", new Wobbler("husks", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["husks"]) },
			{ "ice", new Joinable("ice", new Wobbler[] {
	new Wobbler("ice.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["ice"]), 
	new Wobbler("ice.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["ice"]) }) },
			{ "it", new FacingOnMove(
                name: "it", 
				up: new AnimateOnMove("it", new Wobbler[] {
	new Wobbler("it.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["it"]) }), 
				sleep_up: new AnimateOnMove("it", new Wobbler[] {
	new Wobbler("it.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["it"]) }), 
				left: new AnimateOnMove("it", new Wobbler[] {
	new Wobbler("it.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["it"]) }), 
				sleep_left: new AnimateOnMove("it", new Wobbler[] {
	new Wobbler("it.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["it"]) }), 
				down: new AnimateOnMove("it", new Wobbler[] {
	new Wobbler("it.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["it"]) }), 
				sleep_down: new AnimateOnMove("it", new Wobbler[] {
	new Wobbler("it.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["it"]) }), 
				right: new AnimateOnMove("it", new Wobbler[] {
	new Wobbler("it.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["it"]), 
	new Wobbler("it.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["it"]) }), 
				sleep_right: new AnimateOnMove("it", new Wobbler[] {
	new Wobbler("it.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["it"]) })
) },
			{ "jelly", new Wobbler("jelly", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["jelly"]) },
			{ "jiji", new FacingOnMove(
                name: "jiji", 
				up: new AnimateOnMove("jiji", new Wobbler[] {
	new Wobbler("jiji.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["jiji"]) }), 
				sleep_up: new AnimateOnMove("jiji", new Wobbler[] {
	new Wobbler("jiji.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["jiji"]) }), 
				left: new AnimateOnMove("jiji", new Wobbler[] {
	new Wobbler("jiji.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["jiji"]) }), 
				sleep_left: new AnimateOnMove("jiji", new Wobbler[] {
	new Wobbler("jiji.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["jiji"]) }), 
				down: new AnimateOnMove("jiji", new Wobbler[] {
	new Wobbler("jiji.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["jiji"]) }), 
				sleep_down: new AnimateOnMove("jiji", new Wobbler[] {
	new Wobbler("jiji.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["jiji"]) }), 
				right: new AnimateOnMove("jiji", new Wobbler[] {
	new Wobbler("jiji.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["jiji"]), 
	new Wobbler("jiji.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["jiji"]) }), 
				sleep_right: new AnimateOnMove("jiji", new Wobbler[] {
	new Wobbler("jiji.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["jiji"]) })
) },
			{ "keke", new FacingOnMove(
                name: "keke", 
				up: new AnimateOnMove("keke", new Wobbler[] {
	new Wobbler("keke.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["keke"]) }), 
				sleep_up: new AnimateOnMove("keke", new Wobbler[] {
	new Wobbler("keke.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["keke"]) }), 
				left: new AnimateOnMove("keke", new Wobbler[] {
	new Wobbler("keke.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["keke"]) }), 
				sleep_left: new AnimateOnMove("keke", new Wobbler[] {
	new Wobbler("keke.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["keke"]) }), 
				down: new AnimateOnMove("keke", new Wobbler[] {
	new Wobbler("keke.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["keke"]) }), 
				sleep_down: new AnimateOnMove("keke", new Wobbler[] {
	new Wobbler("keke.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["keke"]) }), 
				right: new AnimateOnMove("keke", new Wobbler[] {
	new Wobbler("keke.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["keke"]), 
	new Wobbler("keke.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["keke"]) }), 
				sleep_right: new AnimateOnMove("keke", new Wobbler[] {
	new Wobbler("keke.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["keke"]) })
) },
			{ "key", new Wobbler("key", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["key"]) },
			{ "knight", new Wobbler("knight", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["knight"]) },
			{ "ladder", new Wobbler("ladder", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["ladder"]) },
			{ "lamp", new Wobbler("lamp", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["lamp"]) },
			{ "lava", new Joinable("lava", new Wobbler[] {
	new Wobbler("lava.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["lava"]), 
	new Wobbler("lava.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["lava"]) }) },
			{ "leaf", new Wobbler("leaf", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["leaf"]) },
			{ "lever", new FacingOnMove(
                name: "lever", 
				up: new AnimateOnMove("lever", new Wobbler[] {
	new Wobbler("lever.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["lever"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("lever", new Wobbler[] {
	new Wobbler("lever.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["lever"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("lever", new Wobbler[] {
	new Wobbler("lever.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["lever"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("lever", new Wobbler[] {
	new Wobbler("lever.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["lever"]) }), 
				sleep_right: null
) },
			{ "lift", new FacingOnMove(
                name: "lift", 
				up: new AnimateOnMove("lift", new Wobbler[] {
	new Wobbler("lift.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["lift"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("lift", new Wobbler[] {
	new Wobbler("lift.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["lift"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("lift", new Wobbler[] {
	new Wobbler("lift.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["lift"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("lift", new Wobbler[] {
	new Wobbler("lift.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["lift"]) }), 
				sleep_right: null
) },
			{ "lily", new Wobbler("lily", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["lily"]) },
			{ "line", new Joinable("line", new Wobbler[] {
	new Wobbler("line.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["line"]), 
	new Wobbler("line.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["line"]) }) },
			{ "lizard", new FacingOnMove(
                name: "lizard", 
				up: new AnimateOnMove("lizard", new Wobbler[] {
	new Wobbler("lizard.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["lizard"]) }), 
				sleep_up: new AnimateOnMove("lizard", new Wobbler[] {
	new Wobbler("lizard.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["lizard"]) }), 
				left: new AnimateOnMove("lizard", new Wobbler[] {
	new Wobbler("lizard.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["lizard"]) }), 
				sleep_left: new AnimateOnMove("lizard", new Wobbler[] {
	new Wobbler("lizard.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["lizard"]) }), 
				down: new AnimateOnMove("lizard", new Wobbler[] {
	new Wobbler("lizard.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["lizard"]) }), 
				sleep_down: new AnimateOnMove("lizard", new Wobbler[] {
	new Wobbler("lizard.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["lizard"]) }), 
				right: new AnimateOnMove("lizard", new Wobbler[] {
	new Wobbler("lizard.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["lizard"]), 
	new Wobbler("lizard.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["lizard"]) }), 
				sleep_right: new AnimateOnMove("lizard", new Wobbler[] {
	new Wobbler("lizard.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["lizard"]) })
) },
			{ "lock", new Wobbler("lock", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["lock"]) },
			{ "love", new Wobbler("love", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["love"]) },
			{ "me", new FacingOnMove(
                name: "me", 
				up: new AnimateOnMove("me", new Wobbler[] {
	new Wobbler("me.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["me"]) }), 
				sleep_up: new AnimateOnMove("me", new Wobbler[] {
	new Wobbler("me.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["me"]) }), 
				left: new AnimateOnMove("me", new Wobbler[] {
	new Wobbler("me.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["me"]) }), 
				sleep_left: new AnimateOnMove("me", new Wobbler[] {
	new Wobbler("me.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["me"]) }), 
				down: new AnimateOnMove("me", new Wobbler[] {
	new Wobbler("me.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["me"]) }), 
				sleep_down: new AnimateOnMove("me", new Wobbler[] {
	new Wobbler("me.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["me"]) }), 
				right: new AnimateOnMove("me", new Wobbler[] {
	new Wobbler("me.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["me"]), 
	new Wobbler("me.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["me"]) }), 
				sleep_right: new AnimateOnMove("me", new Wobbler[] {
	new Wobbler("me.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["me"]) })
) },
			{ "mirror", new Wobbler("mirror", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["mirror"]) },
			{ "monitor", new Wobbler("monitor", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["monitor"]) },
			{ "monster", new FacingOnMove(
                name: "monster", 
				up: new AnimateOnMove("monster", new Wobbler[] {
	new Wobbler("monster.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["monster"]) }), 
				sleep_up: new AnimateOnMove("monster", new Wobbler[] {
	new Wobbler("monster.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["monster"]) }), 
				left: new AnimateOnMove("monster", new Wobbler[] {
	new Wobbler("monster.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["monster"]) }), 
				sleep_left: new AnimateOnMove("monster", new Wobbler[] {
	new Wobbler("monster.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["monster"]) }), 
				down: new AnimateOnMove("monster", new Wobbler[] {
	new Wobbler("monster.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["monster"]) }), 
				sleep_down: new AnimateOnMove("monster", new Wobbler[] {
	new Wobbler("monster.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["monster"]) }), 
				right: new AnimateOnMove("monster", new Wobbler[] {
	new Wobbler("monster.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["monster"]), 
	new Wobbler("monster.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["monster"]) }), 
				sleep_right: new AnimateOnMove("monster", new Wobbler[] {
	new Wobbler("monster.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["monster"]) })
) },
			{ "moon", new Wobbler("moon", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["moon"]) },
			{ "nose", new FacingOnMove(
                name: "nose", 
				up: new AnimateOnMove("nose", new Wobbler[] {
	new Wobbler("nose.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["nose"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("nose", new Wobbler[] {
	new Wobbler("nose.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["nose"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("nose", new Wobbler[] {
	new Wobbler("nose.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["nose"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("nose", new Wobbler[] {
	new Wobbler("nose.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["nose"]) }), 
				sleep_right: null
) },
			{ "orb", new Wobbler("orb", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["orb"]) },
			{ "pants", new Wobbler("pants", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["pants"]) },
			{ "paper", new Wobbler("paper", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["paper"]) },
			{ "pawn", new Wobbler("pawn", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["pawn"]) },
			{ "piano", new FacingOnMove(
                name: "piano", 
				up: new AnimateOnMove("piano", new Wobbler[] {
	new Wobbler("piano.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["piano"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("piano", new Wobbler[] {
	new Wobbler("piano.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["piano"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("piano", new Wobbler[] {
	new Wobbler("piano.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["piano"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("piano", new Wobbler[] {
	new Wobbler("piano.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["piano"]) }), 
				sleep_right: null
) },
			{ "pillar", new Wobbler("pillar", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["pillar"]) },
			{ "pipe", new Joinable("pipe", new Wobbler[] {
	new Wobbler("pipe.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["pipe"]), 
	new Wobbler("pipe.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["pipe"]) }) },
			{ "pixel", new Wobbler("pixel", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["pixel"]) },
			{ "pizza", new Wobbler("pizza", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["pizza"]) },
			{ "plane", new FacingOnMove(
                name: "plane", 
				up: new AnimateOnMove("plane", new Wobbler[] {
	new Wobbler("plane.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["plane"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("plane", new Wobbler[] {
	new Wobbler("plane.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["plane"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("plane", new Wobbler[] {
	new Wobbler("plane.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["plane"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("plane", new Wobbler[] {
	new Wobbler("plane.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["plane"]) }), 
				sleep_right: null
) },
			{ "planet", new Wobbler("planet", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["planet"]) },
			{ "plank", new Joinable("plank", new Wobbler[] {
	new Wobbler("plank.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["plank"]), 
	new Wobbler("plank.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["plank"]) }) },
			{ "potato", new Wobbler("potato", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["potato"]) },
			{ "pumpkin", new FacingOnMove(
                name: "pumpkin", 
				up: new AnimateOnMove("pumpkin", new Wobbler[] {
	new Wobbler("pumpkin.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["pumpkin"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("pumpkin", new Wobbler[] {
	new Wobbler("pumpkin.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["pumpkin"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("pumpkin", new Wobbler[] {
	new Wobbler("pumpkin.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["pumpkin"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("pumpkin", new Wobbler[] {
	new Wobbler("pumpkin.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["pumpkin"]) }), 
				sleep_right: null
) },
			{ "reed", new Wobbler("reed", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["reed"]) },
			{ "ring", new Wobbler("ring", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["ring"]) },
			{ "road", new Joinable("road", new Wobbler[] {
	new Wobbler("road.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["road"]), 
	new Wobbler("road.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["road"]) }) },
			{ "robot", new FacingOnMove(
                name: "robot", 
				up: new AnimateOnMove("robot", new Wobbler[] {
	new Wobbler("robot.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["robot"]) }), 
				sleep_up: new AnimateOnMove("robot", new Wobbler[] {
	new Wobbler("robot.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["robot"]) }), 
				left: new AnimateOnMove("robot", new Wobbler[] {
	new Wobbler("robot.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["robot"]) }), 
				sleep_left: new AnimateOnMove("robot", new Wobbler[] {
	new Wobbler("robot.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["robot"]) }), 
				down: new AnimateOnMove("robot", new Wobbler[] {
	new Wobbler("robot.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["robot"]) }), 
				sleep_down: new AnimateOnMove("robot", new Wobbler[] {
	new Wobbler("robot.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["robot"]) }), 
				right: new AnimateOnMove("robot", new Wobbler[] {
	new Wobbler("robot.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["robot"]), 
	new Wobbler("robot.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["robot"]) }), 
				sleep_right: new AnimateOnMove("robot", new Wobbler[] {
	new Wobbler("robot.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["robot"]) })
) },
			{ "rock", new Wobbler("rock", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["rock"]) },
			{ "rocket", new FacingOnMove(
                name: "rocket", 
				up: new AnimateOnMove("rocket", new Wobbler[] {
	new Wobbler("rocket.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["rocket"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("rocket", new Wobbler[] {
	new Wobbler("rocket.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["rocket"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("rocket", new Wobbler[] {
	new Wobbler("rocket.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["rocket"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("rocket", new Wobbler[] {
	new Wobbler("rocket.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["rocket"]) }), 
				sleep_right: null
) },
			{ "rose", new Wobbler("rose", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["rose"]) },
			{ "rubble", new Joinable("rubble", new Wobbler[] {
	new Wobbler("rubble.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["rubble"]), 
	new Wobbler("rubble.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["rubble"]) }) },
			{ "sax", new FacingOnMove(
                name: "sax", 
				up: new AnimateOnMove("sax", new Wobbler[] {
	new Wobbler("sax.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["sax"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("sax", new Wobbler[] {
	new Wobbler("sax.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["sax"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("sax", new Wobbler[] {
	new Wobbler("sax.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["sax"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("sax", new Wobbler[] {
	new Wobbler("sax.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["sax"]) }), 
				sleep_right: null
) },
			{ "scissors", new FacingOnMove(
                name: "scissors", 
				up: new AnimateOnMove("scissors", new Wobbler[] {
	new Wobbler("scissors.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.up.2", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.up.3", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["scissors"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("scissors", new Wobbler[] {
	new Wobbler("scissors.left.0", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.left.1", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.left.2", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["scissors"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("scissors", new Wobbler[] {
	new Wobbler("scissors.down.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.down.1", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.down.2", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.down.3", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["scissors"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("scissors", new Wobbler[] {
	new Wobbler("scissors.right.0", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.right.1", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.right.2", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["scissors"]), 
	new Wobbler("scissors.right.3", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["scissors"]) }), 
				sleep_right: null
) },
			{ "seed", new Wobbler("seed", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["seed"]) },
			{ "shell", new Wobbler("shell", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["shell"]) },
			{ "shirt", new Wobbler("shirt", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["shirt"]) },
			{ "shovel", new FacingOnMove(
                name: "shovel", 
				up: new AnimateOnMove("shovel", new Wobbler[] {
	new Wobbler("shovel.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["shovel"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("shovel", new Wobbler[] {
	new Wobbler("shovel.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["shovel"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("shovel", new Wobbler[] {
	new Wobbler("shovel.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["shovel"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("shovel", new Wobbler[] {
	new Wobbler("shovel.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["shovel"]) }), 
				sleep_right: null
) },
			{ "sign", new Wobbler("sign", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["sign"]) },
			{ "skull", new FacingOnMove(
                name: "skull", 
				up: new AnimateOnMove("skull", new Wobbler[] {
	new Wobbler("skull.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["skull"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("skull", new Wobbler[] {
	new Wobbler("skull.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["skull"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("skull", new Wobbler[] {
	new Wobbler("skull.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["skull"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("skull", new Wobbler[] {
	new Wobbler("skull.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["skull"]) }), 
				sleep_right: null
) },
			{ "snail", new FacingOnMove(
                name: "snail", 
				up: new AnimateOnMove("snail", new Wobbler[] {
	new Wobbler("snail.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["snail"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("snail", new Wobbler[] {
	new Wobbler("snail.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["snail"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("snail", new Wobbler[] {
	new Wobbler("snail.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["snail"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("snail", new Wobbler[] {
	new Wobbler("snail.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["snail"]) }), 
				sleep_right: null
) },
			{ "spike", new Wobbler("spike", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["spike"]) },
			{ "sprout", new Wobbler("sprout", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["sprout"]) },
			{ "square", new Wobbler("square", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["square"]) },
			{ "star", new Wobbler("star", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["star"]) },
			{ "statue", new FacingOnMove(
                name: "statue", 
				up: new AnimateOnMove("statue", new Wobbler[] {
	new Wobbler("statue.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["statue"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("statue", new Wobbler[] {
	new Wobbler("statue.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["statue"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("statue", new Wobbler[] {
	new Wobbler("statue.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["statue"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("statue", new Wobbler[] {
	new Wobbler("statue.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["statue"]) }), 
				sleep_right: null
) },
			{ "stick", new Wobbler("stick", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["stick"]) },
			{ "stump", new Wobbler("stump", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["stump"]) },
			{ "sun", new Wobbler("sun", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["sun"]) },
			{ "sword", new FacingOnMove(
                name: "sword", 
				up: new AnimateOnMove("sword", new Wobbler[] {
	new Wobbler("sword.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["sword"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("sword", new Wobbler[] {
	new Wobbler("sword.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["sword"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("sword", new Wobbler[] {
	new Wobbler("sword.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["sword"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("sword", new Wobbler[] {
	new Wobbler("sword.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["sword"]) }), 
				sleep_right: null
) },
			{ "table", new Wobbler("table", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["table"]) },
			{ "teeth", new AnimateOnMove("teeth", new Wobbler[] {
	new Wobbler("teeth.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["teeth"]), 
	new Wobbler("teeth.1", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["teeth"]), 
	new Wobbler("teeth.2", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["teeth"]), 
	new Wobbler("teeth.3", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["teeth"]) }) },
			{ "text_0", new Wobbler("text_0", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_0"]) },
			{ "text_1", new Wobbler("text_1", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_1"]) },
			{ "text_2", new Wobbler("text_2", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_2"]) },
			{ "text_3", new Wobbler("text_3", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_3"]) },
			{ "text_3d", new Wobbler("text_3d", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_3d"]) },
			{ "text_4", new Wobbler("text_4", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_4"]) },
			{ "text_5", new Wobbler("text_5", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_5"]) },
			{ "text_6", new Wobbler("text_6", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_6"]) },
			{ "text_7", new Wobbler("text_7", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_7"]) },
			{ "text_8", new Wobbler("text_8", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_8"]) },
			{ "text_9", new Wobbler("text_9", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_9"]) },
			{ "text_a", new Wobbler("text_a", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_a"]) },
			{ "text_above", new Wobbler("text_above", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_above"]) },
			{ "text_algae", new Wobbler("text_algae", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_algae"]) },
			{ "text_all", new Wobbler("text_all", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_all"]) },
			{ "text_and", new Wobbler("text_and", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_and"]) },
			{ "text_apos", new Wobbler("text_apos", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_apos"]) },
			{ "text_arm", new Wobbler("text_arm", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_arm"]) },
			{ "text_arrow", new Wobbler("text_arrow", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_arrow"]) },
			{ "text_auto", new Wobbler("text_auto", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_auto"]) },
			{ "text_b", new Wobbler("text_b", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_b"]) },
			{ "text_baba", new Wobbler("text_baba", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_baba"]) },
			{ "text_back", new Wobbler("text_back", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_back"]) },
			{ "text_badbad", new Wobbler("text_badbad", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_badbad"]) },
			{ "text_banana", new Wobbler("text_banana", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_banana"]) },
			{ "text_bat", new Wobbler("text_bat", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bat"]) },
			{ "text_bed", new Wobbler("text_bed", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bed"]) },
			{ "text_bee", new Wobbler("text_bee", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bee"]) },
			{ "text_below", new Wobbler("text_below", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_below"]) },
			{ "text_belt", new Wobbler("text_belt", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_belt"]) },
			{ "text_besideleft", new Wobbler("text_besideleft", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_besideleft"]) },
			{ "text_besideright", new Wobbler("text_besideright", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_besideright"]) },
			{ "text_best", new Wobbler("text_best", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_best"]) },
			{ "text_bird", new Wobbler("text_bird", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bird"]) },
			{ "text_black", new Wobbler("text_black", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_black"]) },
			{ "text_blob", new Wobbler("text_blob", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_blob"]) },
			{ "text_blue", new Wobbler("text_blue", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_blue"]) },
			{ "text_boat", new Wobbler("text_boat", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_boat"]) },
			{ "text_boba", new Wobbler("text_boba", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_boba"]) },
			{ "text_bog", new Wobbler("text_bog", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bog"]) },
			{ "text_bolt", new Wobbler("text_bolt", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bolt"]) },
			{ "text_bomb", new Wobbler("text_bomb", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bomb"]) },
			{ "text_bonus", new Wobbler("text_bonus", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bonus"]) },
			{ "text_book", new Wobbler("text_book", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_book"]) },
			{ "text_boom", new Wobbler("text_boom", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_boom"]) },
			{ "text_bottle", new Wobbler("text_bottle", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bottle"]) },
			{ "text_box", new Wobbler("text_box", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_box"]) },
			{ "text_brick", new Wobbler("text_brick", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_brick"]) },
			{ "text_broken", new Wobbler("text_broken", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_broken"]) },
			{ "text_brown", new Wobbler("text_brown", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_brown"]) },
			{ "text_bubble", new Wobbler("text_bubble", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bubble"]) },
			{ "text_bucket", new Wobbler("text_bucket", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bucket"]) },
			{ "text_bug", new Wobbler("text_bug", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bug"]) },
			{ "text_bunny", new Wobbler("text_bunny", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_bunny"]) },
			{ "text_burger", new Wobbler("text_burger", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_burger"]) },
			{ "text_c", new Wobbler("text_c", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_c"]) },
			{ "text_cake", new Wobbler("text_cake", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cake"]) },
			{ "text_car", new Wobbler("text_car", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_car"]) },
			{ "text_cart", new Wobbler("text_cart", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cart"]) },
			{ "text_cash", new Wobbler("text_cash", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cash"]) },
			{ "text_cat", new Wobbler("text_cat", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cat"]) },
			{ "text_chair", new Wobbler("text_chair", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_chair"]) },
			{ "text_cheese", new Wobbler("text_cheese", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cheese"]) },
			{ "text_chill", new Wobbler("text_chill", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_chill"]) },
			{ "text_circle", new Wobbler("text_circle", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_circle"]) },
			{ "text_cliff", new Wobbler("text_cliff", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cliff"]) },
			{ "text_clock", new Wobbler("text_clock", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_clock"]) },
			{ "text_cloud", new Wobbler("text_cloud", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cloud"]) },
			{ "text_cog", new Wobbler("text_cog", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cog"]) },
			{ "text_colon", new Wobbler("text_colon", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_colon"]) },
			{ "text_comma", new Wobbler("text_comma", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_comma"]) },
			{ "text_crab", new Wobbler("text_crab", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_crab"]) },
			{ "text_crystal", new Wobbler("text_crystal", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_crystal"]) },
			{ "text_ctrl", new Wobbler("text_ctrl", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_ctrl"]) },
			{ "text_cup", new Wobbler("text_cup", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cup"]) },
			{ "text_cursor", new Wobbler("text_cursor", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cursor"]) },
			{ "text_cyan", new Wobbler("text_cyan", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_cyan"]) },
			{ "text_d", new Wobbler("text_d", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_d"]) },
			{ "text_defeat", new Wobbler("text_defeat", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_defeat"]) },
			{ "text_deturn", new Wobbler("text_deturn", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_deturn"]) },
			{ "text_dog", new Wobbler("text_dog", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_dog"]) },
			{ "text_done", new Wobbler("text_done", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_done"]) },
			{ "text_donut", new Wobbler("text_donut", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_donut"]) },
			{ "text_door", new Wobbler("text_door", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_door"]) },
			{ "text_dot", new Wobbler("text_dot", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_dot"]) },
			{ "text_down", new Wobbler("text_down", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_down"]) },
			{ "text_drink", new Wobbler("text_drink", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_drink"]) },
			{ "text_drum", new Wobbler("text_drum", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_drum"]) },
			{ "text_dust", new Wobbler("text_dust", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_dust"]) },
			{ "text_e", new Wobbler("text_e", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_e"]) },
			{ "text_ear", new Wobbler("text_ear", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_ear"]) },
			{ "text_eat", new Wobbler("text_eat", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_eat"]) },
			{ "text_egg", new Wobbler("text_egg", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_egg"]) },
			{ "text_empty", new Wobbler("text_empty", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_empty"]) },
			{ "text_end", new Wobbler("text_end", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_end"]) },
			{ "text_escape", new Wobbler("text_escape", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_escape"]) },
			{ "text_eye", new Wobbler("text_eye", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_eye"]) },
			{ "text_f", new Wobbler("text_f", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_f"]) },
			{ "text_facing", new Wobbler("text_facing", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_facing"]) },
			{ "text_fall", new Wobbler("text_fall", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_fall"]) },
			{ "text_fallleft", new Wobbler("text_fallleft", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_fallleft"]) },
			{ "text_fallright", new Wobbler("text_fallright", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_fallright"]) },
			{ "text_fallup", new Wobbler("text_fallup", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_fallup"]) },
			{ "text_fear", new Wobbler("text_fear", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_fear"]) },
			{ "text_feeling", new Wobbler("text_feeling", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_feeling"]) },
			{ "text_fence", new Wobbler("text_fence", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_fence"]) },
			{ "text_fire", new Wobbler("text_fire", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_fire"]) },
			{ "text_fish", new Wobbler("text_fish", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_fish"]) },
			{ "text_flag", new Wobbler("text_flag", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_flag"]) },
			{ "text_flat", new Wobbler("text_flat", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_flat"]) },
			{ "text_float", new Wobbler("text_float", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_float"]) },
			{ "text_flower", new Wobbler("text_flower", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_flower"]) },
			{ "text_fofo", new Wobbler("text_fofo", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_fofo"]) },
			{ "text_foliage", new Wobbler("text_foliage", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_foliage"]) },
			{ "text_follow", new Wobbler("text_follow", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_follow"]) },
			{ "text_foot", new Wobbler("text_foot", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_foot"]) },
			{ "text_fort", new Wobbler("text_fort", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_fort"]) },
			{ "text_frog", new Wobbler("text_frog", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_frog"]) },
			{ "text_fruit", new Wobbler("text_fruit", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_fruit"]) },
			{ "text_fungi", new Wobbler("text_fungi", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_fungi"]) },
			{ "text_fungus", new Wobbler("text_fungus", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_fungus"]) },
			{ "text_fwslash", new Wobbler("text_fwslash", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_fwslash"]) },
			{ "text_g", new Wobbler("text_g", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_g"]) },
			{ "text_gate", new Wobbler("text_gate", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_gate"]) },
			{ "text_gem", new Wobbler("text_gem", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_gem"]) },
			{ "text_ghost", new Wobbler("text_ghost", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_ghost"]) },
			{ "text_grab", new Wobbler("text_grab", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_grab"]) },
			{ "text_grass", new Wobbler("text_grass", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_grass"]) },
			{ "text_green", new Wobbler("text_green", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_green"]) },
			{ "text_grey", new Wobbler("text_grey", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_grey"]) },
			{ "text_group", new Wobbler("text_group", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_group"]) },
			{ "text_group2", new Wobbler("text_group2", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_group2"]) },
			{ "text_group3", new Wobbler("text_group3", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_group3"]) },
			{ "text_guitar", new Wobbler("text_guitar", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_guitar"]) },
			{ "text_h", new Wobbler("text_h", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_h"]) },
			{ "text_hand", new Wobbler("text_hand", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_hand"]) },
			{ "text_has", new Wobbler("text_has", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_has"]) },
			{ "text_hedge", new Wobbler("text_hedge", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_hedge"]) },
			{ "text_hide", new Wobbler("text_hide", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_hide"]) },
			{ "text_hihat", new Wobbler("text_hihat", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_hihat"]) },
			{ "text_hot", new Wobbler("text_hot", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_hot"]) },
			{ "text_house", new Wobbler("text_house", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_house"]) },
			{ "text_husk", new Wobbler("text_husk", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_husk"]) },
			{ "text_husks", new Wobbler("text_husks", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_husks"]) },
			{ "text_hyphen", new Wobbler("text_hyphen", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_hyphen"]) },
			{ "text_i", new Wobbler("text_i", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_i"]) },
			{ "text_ice", new Wobbler("text_ice", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_ice"]) },
			{ "text_idle", new Wobbler("text_idle", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_idle"]) },
			{ "text_is", new Wobbler("text_is", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_is"]) },
			{ "text_it", new Wobbler("text_it", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_it"]) },
			{ "text_j", new Wobbler("text_j", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_j"]) },
			{ "text_jelly", new Wobbler("text_jelly", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_jelly"]) },
			{ "text_jiji", new Wobbler("text_jiji", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_jiji"]) },
			{ "text_k", new Wobbler("text_k", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_k"]) },
			{ "text_keke", new Wobbler("text_keke", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_keke"]) },
			{ "text_key", new Wobbler("text_key", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_key"]) },
			{ "text_knight", new Wobbler("text_knight", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_knight"]) },
			{ "text_l", new Wobbler("text_l", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_l"]) },
			{ "text_ladder", new Wobbler("text_ladder", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_ladder"]) },
			{ "text_lamp", new Wobbler("text_lamp", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_lamp"]) },
			{ "text_lava", new Wobbler("text_lava", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_lava"]) },
			{ "text_leaf", new Wobbler("text_leaf", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_leaf"]) },
			{ "text_left", new Wobbler("text_left", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_left"]) },
			{ "text_level", new Wobbler("text_level", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_level"]) },
			{ "text_lever", new Wobbler("text_lever", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_lever"]) },
			{ "text_lift", new Wobbler("text_lift", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_lift"]) },
			{ "text_lily", new Wobbler("text_lily", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_lily"]) },
			{ "text_lime", new Wobbler("text_lime", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_lime"]) },
			{ "text_line", new Wobbler("text_line", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_line"]) },
			{ "text_lizard", new Wobbler("text_lizard", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_lizard"]) },
			{ "text_lock", new Wobbler("text_lock", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_lock"]) },
			{ "text_lockeddown", new Wobbler("text_lockeddown", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_lockeddown"]) },
			{ "text_lockedleft", new Wobbler("text_lockedleft", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_lockedleft"]) },
			{ "text_lockedright", new Wobbler("text_lockedright", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_lockedright"]) },
			{ "text_lockedup", new Wobbler("text_lockedup", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_lockedup"]) },
			{ "text_lonely", new Wobbler("text_lonely", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_lonely"]) },
			{ "text_love", new Wobbler("text_love", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_love"]) },
			{ "text_lsqbr", new Wobbler("text_lsqbr", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_lsqbr"]) },
			{ "text_m", new Wobbler("text_m", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_m"]) },
			{ "text_make", new Wobbler("text_make", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_make"]) },
			{ "text_me", new Wobbler("text_me", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_me"]) },
			{ "text_melt", new Wobbler("text_melt", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_melt"]) },
			{ "text_mimic", new Wobbler("text_mimic", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_mimic"]) },
			{ "text_mirror", new Wobbler("text_mirror", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_mirror"]) },
			{ "text_monitor", new Wobbler("text_monitor", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_monitor"]) },
			{ "text_monster", new Wobbler("text_monster", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_monster"]) },
			{ "text_moon", new Wobbler("text_moon", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_moon"]) },
			{ "text_more", new Wobbler("text_more", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_more"]) },
			{ "text_move", new Wobbler("text_move", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_move"]) },
			{ "text_n", new Wobbler("text_n", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_n"]) },
			{ "text_near", new Wobbler("text_near", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_near"]) },
			{ "text_nextto", new Wobbler("text_nextto", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_nextto"]) },
			{ "text_nose", new Wobbler("text_nose", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_nose"]) },
			{ "text_not", new Wobbler("text_not", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_not"]) },
			{ "text_nudgedown", new Wobbler("text_nudgedown", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_nudgedown"]) },
			{ "text_nudgeleft", new Wobbler("text_nudgeleft", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_nudgeleft"]) },
			{ "text_nudgeright", new Wobbler("text_nudgeright", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_nudgeright"]) },
			{ "text_nudgeup", new Wobbler("text_nudgeup", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_nudgeup"]) },
			{ "text_o", new Wobbler("text_o", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_o"]) },
			{ "text_often", new Wobbler("text_often", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_often"]) },
			{ "text_on", new Wobbler("text_on", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_on"]) },
			{ "text_open", new Wobbler("text_open", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_open"]) },
			{ "text_orange", new Wobbler("text_orange", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_orange"]) },
			{ "text_orb", new Wobbler("text_orb", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_orb"]) },
			{ "text_p", new Wobbler("text_p", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_p"]) },
			{ "text_pants", new Wobbler("text_pants", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pants"]) },
			{ "text_paper", new Wobbler("text_paper", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_paper"]) },
			{ "text_party", new Wobbler("text_party", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_party"]) },
			{ "text_pawn", new Wobbler("text_pawn", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pawn"]) },
			{ "text_pet", new Wobbler("text_pet", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pet"]) },
			{ "text_phantom", new Wobbler("text_phantom", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_phantom"]) },
			{ "text_piano", new Wobbler("text_piano", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_piano"]) },
			{ "text_pillar", new Wobbler("text_pillar", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pillar"]) },
			{ "text_pink", new Wobbler("text_pink", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pink"]) },
			{ "text_pipe", new Wobbler("text_pipe", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pipe"]) },
			{ "text_pixel", new Wobbler("text_pixel", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pixel"]) },
			{ "text_pizza", new Wobbler("text_pizza", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pizza"]) },
			{ "text_plane", new Wobbler("text_plane", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_plane"]) },
			{ "text_planet", new Wobbler("text_planet", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_planet"]) },
			{ "text_plank", new Wobbler("text_plank", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_plank"]) },
			{ "text_play", new Wobbler("text_play", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_play"]) },
			{ "text_plus", new Wobbler("text_plus", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_plus"]) },
			{ "text_potato", new Wobbler("text_potato", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_potato"]) },
			{ "text_power", new Wobbler("text_power", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_power"]) },
			{ "text_power2", new Wobbler("text_power2", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_power2"]) },
			{ "text_power3", new Wobbler("text_power3", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_power3"]) },
			{ "text_powered", new Wobbler("text_powered", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_powered"]) },
			{ "text_powered2", new Wobbler("text_powered2", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_powered2"]) },
			{ "text_powered3", new Wobbler("text_powered3", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_powered3"]) },
			{ "text_pull", new Wobbler("text_pull", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pull"]) },
			{ "text_pumpkin", new Wobbler("text_pumpkin", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_pumpkin"]) },
			{ "text_purple", new Wobbler("text_purple", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_purple"]) },
			{ "text_push", new Wobbler("text_push", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_push"]) },
			{ "text_q", new Wobbler("text_q", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_q"]) },
			{ "text_quote", new Wobbler("text_quote", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_quote"]) },
			{ "text_r", new Wobbler("text_r", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_r"]) },
			{ "text_red", new Wobbler("text_red", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_red"]) },
			{ "text_reed", new Wobbler("text_reed", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_reed"]) },
			{ "text_reverse", new Wobbler("text_reverse", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_reverse"]) },
			{ "text_revert", new Wobbler("text_revert", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_revert"]) },
			{ "text_right", new Wobbler("text_right", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_right"]) },
			{ "text_ring", new Wobbler("text_ring", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_ring"]) },
			{ "text_road", new Wobbler("text_road", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_road"]) },
			{ "text_robot", new Wobbler("text_robot", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_robot"]) },
			{ "text_rock", new Wobbler("text_rock", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_rock"]) },
			{ "text_rocket", new Wobbler("text_rocket", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_rocket"]) },
			{ "text_rose", new Wobbler("text_rose", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_rose"]) },
			{ "text_rosy", new Wobbler("text_rosy", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_rosy"]) },
			{ "text_rsqbr", new Wobbler("text_rsqbr", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_rsqbr"]) },
			{ "text_rubble", new Wobbler("text_rubble", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_rubble"]) },
			{ "text_s", new Wobbler("text_s", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_s"]) },
			{ "text_sad", new Wobbler("text_sad", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_sad"]) },
			{ "text_safe", new Wobbler("text_safe", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_safe"]) },
			{ "text_sax", new Wobbler("text_sax", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_sax"]) },
			{ "text_scissors", new Wobbler("text_scissors", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_scissors"]) },
			{ "text_seed", new Wobbler("text_seed", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_seed"]) },
			{ "text_seeing", new Wobbler("text_seeing", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_seeing"]) },
			{ "text_seldom", new Wobbler("text_seldom", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_seldom"]) },
			{ "text_select", new Wobbler("text_select", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_select"]) },
			{ "text_sharp", new Wobbler("text_sharp", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_sharp"]) },
			{ "text_shell", new Wobbler("text_shell", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_shell"]) },
			{ "text_shift", new Wobbler("text_shift", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_shift"]) },
			{ "text_shirt", new Wobbler("text_shirt", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_shirt"]) },
			{ "text_shovel", new Wobbler("text_shovel", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_shovel"]) },
			{ "text_shut", new Wobbler("text_shut", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_shut"]) },
			{ "text_sign", new Wobbler("text_sign", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_sign"]) },
			{ "text_silver", new Wobbler("text_silver", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_silver"]) },
			{ "text_sink", new Wobbler("text_sink", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_sink"]) },
			{ "text_skull", new Wobbler("text_skull", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_skull"]) },
			{ "text_sleep", new Wobbler("text_sleep", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_sleep"]) },
			{ "text_snail", new Wobbler("text_snail", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_snail"]) },
			{ "text_spike", new Wobbler("text_spike", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_spike"]) },
			{ "text_sprout", new Wobbler("text_sprout", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_sprout"]) },
			{ "text_square", new Wobbler("text_square", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_square"]) },
			{ "text_star", new Wobbler("text_star", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_star"]) },
			{ "text_statue", new Wobbler("text_statue", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_statue"]) },
			{ "text_stick", new Wobbler("text_stick", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_stick"]) },
			{ "text_still", new Wobbler("text_still", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_still"]) },
			{ "text_stop", new Wobbler("text_stop", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_stop"]) },
			{ "text_stump", new Wobbler("text_stump", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_stump"]) },
			{ "text_sun", new Wobbler("text_sun", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_sun"]) },
			{ "text_swap", new Wobbler("text_swap", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_swap"]) },
			{ "text_sword", new Wobbler("text_sword", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_sword"]) },
			{ "text_t", new Wobbler("text_t", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_t"]) },
			{ "text_table", new Wobbler("text_table", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_table"]) },
			{ "text_teeth", new Wobbler("text_teeth", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_teeth"]) },
			{ "text_tele", new Wobbler("text_tele", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_tele"]) },
			{ "text_text", new Wobbler("text_text", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_text"]) },
			{ "text_tile", new Wobbler("text_tile", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_tile"]) },
			{ "text_tower", new Wobbler("text_tower", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_tower"]) },
			{ "text_track", new Wobbler("text_track", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_track"]) },
			{ "text_train", new Wobbler("text_train", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_train"]) },
			{ "text_tree", new Wobbler("text_tree", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_tree"]) },
			{ "text_trees", new Wobbler("text_trees", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_trees"]) },
			{ "text_triangle", new Wobbler("text_triangle", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_triangle"]) },
			{ "text_trumpet", new Wobbler("text_trumpet", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_trumpet"]) },
			{ "text_turn", new Wobbler("text_turn", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(30, 30), sheets["text_turn"]) },
			{ "text_turnip", new Wobbler("text_turnip", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_turnip"]) },
			{ "text_turtle", new Wobbler("text_turtle", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_turtle"]) },
			{ "text_u", new Wobbler("text_u", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_u"]) },
			{ "text_ufo", new Wobbler("text_ufo", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_ufo"]) },
			{ "text_underscore", new Wobbler("text_underscore", new[] { new Point(0, 0), new Point(1, 0) }, new Point(24, 24), sheets["text_underscore"]) },
			{ "text_up", new Wobbler("text_up", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_up"]) },
			{ "text_v", new Wobbler("text_v", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_v"]) },
			{ "text_vase", new Wobbler("text_vase", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_vase"]) },
			{ "text_vine", new Wobbler("text_vine", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_vine"]) },
			{ "text_w", new Wobbler("text_w", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_w"]) },
			{ "text_wall", new Wobbler("text_wall", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_wall"]) },
			{ "text_water", new Wobbler("text_water", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_water"]) },
			{ "text_weak", new Wobbler("text_weak", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_weak"]) },
			{ "text_what", new Wobbler("text_what", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_what"]) },
			{ "text_white", new Wobbler("text_white", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_white"]) },
			{ "text_win", new Wobbler("text_win", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_win"]) },
			{ "text_wind", new Wobbler("text_wind", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_wind"]) },
			{ "text_without", new Wobbler("text_without", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_without"]) },
			{ "text_wonder", new Wobbler("text_wonder", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_wonder"]) },
			{ "text_word", new Wobbler("text_word", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_word"]) },
			{ "text_worm", new Wobbler("text_worm", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_worm"]) },
			{ "text_write", new Wobbler("text_write", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_write"]) },
			{ "text_x", new Wobbler("text_x", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_x"]) },
			{ "text_y", new Wobbler("text_y", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_y"]) },
			{ "text_yellow", new Wobbler("text_yellow", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_yellow"]) },
			{ "text_you", new Wobbler("text_you", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_you"]) },
			{ "text_you2", new Wobbler("text_you2", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_you2"]) },
			{ "text_z", new Wobbler("text_z", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["text_z"]) },
			{ "tile", new Wobbler("tile", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["tile"]) },
			{ "tower", new Wobbler("tower", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["tower"]) },
			{ "track", new Joinable("track", new Wobbler[] {
	new Wobbler("track.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["track"]), 
	new Wobbler("track.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["track"]) }) },
			{ "train", new FacingOnMove(
                name: "train", 
				up: new AnimateOnMove("train", new Wobbler[] {
	new Wobbler("train.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["train"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("train", new Wobbler[] {
	new Wobbler("train.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["train"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("train", new Wobbler[] {
	new Wobbler("train.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["train"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("train", new Wobbler[] {
	new Wobbler("train.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["train"]) }), 
				sleep_right: null
) },
			{ "tree", new Wobbler("tree", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["tree"]) },
			{ "trees", new Wobbler("trees", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["trees"]) },
			{ "triangle", new Wobbler("triangle", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["triangle"]) },
			{ "trumpet", new FacingOnMove(
                name: "trumpet", 
				up: new AnimateOnMove("trumpet", new Wobbler[] {
	new Wobbler("trumpet.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["trumpet"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("trumpet", new Wobbler[] {
	new Wobbler("trumpet.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["trumpet"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("trumpet", new Wobbler[] {
	new Wobbler("trumpet.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["trumpet"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("trumpet", new Wobbler[] {
	new Wobbler("trumpet.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["trumpet"]) }), 
				sleep_right: null
) },
			{ "turnip", new Wobbler("turnip", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["turnip"]) },
			{ "turtle", new FacingOnMove(
                name: "turtle", 
				up: new AnimateOnMove("turtle", new Wobbler[] {
	new Wobbler("turtle.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["turtle"]) }), 
				sleep_up: new AnimateOnMove("turtle", new Wobbler[] {
	new Wobbler("turtle.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["turtle"]) }), 
				left: new AnimateOnMove("turtle", new Wobbler[] {
	new Wobbler("turtle.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["turtle"]) }), 
				sleep_left: new AnimateOnMove("turtle", new Wobbler[] {
	new Wobbler("turtle.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["turtle"]) }), 
				down: new AnimateOnMove("turtle", new Wobbler[] {
	new Wobbler("turtle.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["turtle"]) }), 
				sleep_down: new AnimateOnMove("turtle", new Wobbler[] {
	new Wobbler("turtle.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["turtle"]) }), 
				right: new AnimateOnMove("turtle", new Wobbler[] {
	new Wobbler("turtle.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["turtle"]), 
	new Wobbler("turtle.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["turtle"]) }), 
				sleep_right: new AnimateOnMove("turtle", new Wobbler[] {
	new Wobbler("turtle.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["turtle"]) })
) },
			{ "ufo", new Wobbler("ufo", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["ufo"]) },
			{ "vase", new Wobbler("vase", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["vase"]) },
			{ "vine", new Joinable("vine", new Wobbler[] {
	new Wobbler("vine.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["vine"]), 
	new Wobbler("vine.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["vine"]) }) },
			{ "wall", new Joinable("wall", new Wobbler[] {
	new Wobbler("wall.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["wall"]), 
	new Wobbler("wall.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["wall"]) }) },
			{ "water", new Joinable("water", new Wobbler[] {
	new Wobbler("water.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.r", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.u", new[] { new Point(6, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.ur", new[] { new Point(2, 1), new Point(3, 1), new Point(4, 1) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.l", new[] { new Point(5, 1), new Point(6, 1), new Point(0, 2) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.lr", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.ul", new[] { new Point(4, 2), new Point(5, 2), new Point(6, 2) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.ulr", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.d", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.dr", new[] { new Point(6, 3), new Point(0, 4), new Point(1, 4) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.ud", new[] { new Point(2, 4), new Point(3, 4), new Point(4, 4) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.udr", new[] { new Point(5, 4), new Point(6, 4), new Point(0, 5) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.dl", new[] { new Point(1, 5), new Point(2, 5), new Point(3, 5) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.dlr", new[] { new Point(4, 5), new Point(5, 5), new Point(6, 5) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.udl", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["water"]), 
	new Wobbler("water.udlr", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["water"]) }) },
			{ "what", new Wobbler("what", new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1) }, new Point(24, 24), sheets["what"]) },
			{ "wind", new FacingOnMove(
                name: "wind", 
				up: new AnimateOnMove("wind", new Wobbler[] {
	new Wobbler("wind.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["wind"]) }), 
				sleep_up: null, 
				left: new AnimateOnMove("wind", new Wobbler[] {
	new Wobbler("wind.left.0", new[] { new Point(3, 0), new Point(0, 1), new Point(1, 1) }, new Point(24, 24), sheets["wind"]) }), 
				sleep_left: null, 
				down: new AnimateOnMove("wind", new Wobbler[] {
	new Wobbler("wind.down.0", new[] { new Point(2, 1), new Point(3, 1), new Point(0, 2) }, new Point(24, 24), sheets["wind"]) }), 
				sleep_down: null, 
				right: new AnimateOnMove("wind", new Wobbler[] {
	new Wobbler("wind.right.0", new[] { new Point(1, 2), new Point(2, 2), new Point(3, 2) }, new Point(24, 24), sheets["wind"]) }), 
				sleep_right: null
) },
			{ "worm", new FacingOnMove(
                name: "worm", 
				up: new AnimateOnMove("worm", new Wobbler[] {
	new Wobbler("worm.up.0", new[] { new Point(0, 0), new Point(1, 0), new Point(2, 0) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.up.1", new[] { new Point(3, 0), new Point(4, 0), new Point(5, 0) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.up.2", new[] { new Point(6, 0), new Point(7, 0), new Point(0, 1) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.up.3", new[] { new Point(1, 1), new Point(2, 1), new Point(3, 1) }, new Point(24, 24), sheets["worm"]) }), 
				sleep_up: new AnimateOnMove("worm", new Wobbler[] {
	new Wobbler("worm.sleep_up.0", new[] { new Point(4, 1), new Point(5, 1), new Point(6, 1) }, new Point(24, 24), sheets["worm"]) }), 
				left: new AnimateOnMove("worm", new Wobbler[] {
	new Wobbler("worm.left.0", new[] { new Point(7, 1), new Point(0, 2), new Point(1, 2) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.left.1", new[] { new Point(2, 2), new Point(3, 2), new Point(4, 2) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.left.2", new[] { new Point(5, 2), new Point(6, 2), new Point(7, 2) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.left.3", new[] { new Point(0, 3), new Point(1, 3), new Point(2, 3) }, new Point(24, 24), sheets["worm"]) }), 
				sleep_left: new AnimateOnMove("worm", new Wobbler[] {
	new Wobbler("worm.sleep_left.0", new[] { new Point(3, 3), new Point(4, 3), new Point(5, 3) }, new Point(24, 24), sheets["worm"]) }), 
				down: new AnimateOnMove("worm", new Wobbler[] {
	new Wobbler("worm.down.0", new[] { new Point(6, 3), new Point(7, 3), new Point(0, 4) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.down.1", new[] { new Point(1, 4), new Point(2, 4), new Point(3, 4) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.down.2", new[] { new Point(4, 4), new Point(5, 4), new Point(6, 4) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.down.3", new[] { new Point(7, 4), new Point(0, 5), new Point(1, 5) }, new Point(24, 24), sheets["worm"]) }), 
				sleep_down: new AnimateOnMove("worm", new Wobbler[] {
	new Wobbler("worm.sleep_down.0", new[] { new Point(2, 5), new Point(3, 5), new Point(4, 5) }, new Point(24, 24), sheets["worm"]) }), 
				right: new AnimateOnMove("worm", new Wobbler[] {
	new Wobbler("worm.right.0", new[] { new Point(5, 5), new Point(6, 5), new Point(7, 5) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.right.1", new[] { new Point(0, 6), new Point(1, 6), new Point(2, 6) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.right.2", new[] { new Point(3, 6), new Point(4, 6), new Point(5, 6) }, new Point(24, 24), sheets["worm"]), 
	new Wobbler("worm.right.3", new[] { new Point(6, 6), new Point(7, 6), new Point(0, 7) }, new Point(24, 24), sheets["worm"]) }), 
				sleep_right: new AnimateOnMove("worm", new Wobbler[] {
	new Wobbler("worm.sleep_right.0", new[] { new Point(1, 7), new Point(2, 7), new Point(3, 7) }, new Point(24, 24), sheets["worm"]) })
) }
            };
        }
    }
}
