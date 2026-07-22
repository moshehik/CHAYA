# Goal Description

The user requested significant layout and styling changes to the `drop-card` inflatable rings:
1. Make the inflatable float ring **much thinner**.
2. Move the SVG animation to be "drawn on the ring" and "curving with it", in a dark gray color.
3. Move the title (`<h4>`) to be written ON the ring itself.
4. Only the description (`<p>`) should remain inside the center hole of the ring.

## Proposed Changes

### 1. Thinner Ring
To make the tube of the ring thinner without distorting the global lighting, we will adjust the `::before` pseudo-element's `background-size` and positioning, or use CSS `border-image` to slice the transparent PNG and explicitly define a thin `border-width`. The `border-image` approach ensures the corners remain perfectly rounded while the straight sides can be as thin as we want (e.g., `30px`).

### 2. Title and SVG Re-positioning
We will update the HTML layout of the `.drop-card`:
- The `<svg>` line art will be positioned absolutely to sit directly on the top border of the ring.
- The `<h4>` title will also be positioned absolutely, likely on the bottom border of the ring.
- Both elements will be styled to look like they are printed/drawn on the inflatable plastic. For the SVG, we will change its stroke color to dark gray (`#333`) and add a slight CSS curved distortion if possible, or use an SVG filter (like `feDisplacementMap` or simply 3D transforms) to make it look wrapped around the tube's curvature.
- The `<p>` description will remain in the normal flow inside the `.drop-card`, centered within the white background hole.

## Open Questions

> [!IMPORTANT]
> **User Feedback Needed:**
> 1. By "curving with it" (שתתעגל איתו), do you mean you want the text and the icon to physically curve in a circle/arc along the edge, or do you mean you want a 3D effect so it looks like it's printed wrapped around the round surface of the tube?
> 2. Should the title be at the top of the ring and the animation at the bottom, or vice versa?

## Verification Plan

1. Modify `styles.css` and `index.html`.
2. Open the page locally to ensure the ring is thin.
3. Verify that the title and SVG perfectly align on the border of the ring.
4. Verify the SVG animations still trigger correctly.
