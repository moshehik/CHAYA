document.addEventListener('DOMContentLoaded', () => {
    
    // Intersection Observer for Scroll Animations & Luxury Transitions
    const observerOptions = {
        root: null,
        rootMargin: '0px',
        threshold: 0.15
    };

    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                // Add class to trigger animation (clip-paths, fades, line-drawing)
                entry.target.classList.add('is-visible');
                
                // If the element has a custom delay, apply it
                const delay = entry.target.style.getPropertyValue('--delay');
                if (delay) {
                    entry.target.style.transitionDelay = delay;
                }
                
                // Stop observing once animated so the drawing/transition doesn't repeat backwards
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Observe all animateable elements
    const animatedElements = document.querySelectorAll('.animate-on-scroll, .clip-transition, .draw-line-svg');
    animatedElements.forEach(el => observer.observe(el));

    // Smooth Scrolling for Anchor Links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            if (targetId === '#') return;
            
            const targetElement = document.querySelector(targetId);
            if (targetElement) {
                targetElement.scrollIntoView({
                    behavior: 'smooth'
                });
            }
        });
    });
});
