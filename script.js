document.addEventListener('DOMContentLoaded', () => {
    
    // Character wrapping for continuous reading animation
    const readingBoxes = document.querySelectorAll('.animated-reading-box');
    readingBoxes.forEach(box => {
        const text = box.textContent.trim();
        box.innerHTML = '';
        const chars = Array.from(text);
        chars.forEach((char, index) => {
            const span = document.createElement('span');
            span.textContent = char;
            span.style.setProperty('--char-index', index);
            box.appendChild(span);
        });
    });

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
    const animatedElements = document.querySelectorAll('.animate-on-scroll, .clip-transition, .draw-line-svg, #provider-section');
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

    // Premium Parallax Effect for Background Elements
    const morphContainer = document.querySelector('.morph-container');
    const bubblesContainer = document.querySelector('.bubbles-container');
    
    window.addEventListener('scroll', () => {
        const scrolled = window.pageYOffset;
        
        if (morphContainer) {
            morphContainer.style.transform = `translateY(${scrolled * 0.15}px)`;
        }
        
        if (bubblesContainer) {
            bubblesContainer.style.transform = `translateY(${scrolled * 0.05}px)`;
        }
    });

    // Stream Bubbles Logic (Luxurious Soap Bubbles Flowing on Waves)
    function spawnStreamBubble() {
        const container = document.getElementById('water-stream-container');
        if (!container) return;

        const bubble = document.createElement('div');
        bubble.classList.add('stream-bubble');
        
        // Random start position along the width
        const startLeft = Math.random() * 100;
        // Start near the bottom/middle of the waves
        const startBottom = Math.random() * 40 + 10; 
        
        // Luxurious bubbles in smaller sizes: from 8px up to 35px
        const size = Math.random() * 27 + 8; 
        
        // Slower, elegant duration
        const duration = Math.random() * 6 + 4; 
        
        // Drift along the waves (right to left for Hebrew layout)
        const driftXEnd = -(Math.random() * 200 + 100); 
        
        // Bobbing up and down amount
        const bobY = (Math.random() - 0.5) * 40; 
        
        bubble.style.left = `${startLeft}%`;
        bubble.style.bottom = `${startBottom}px`;
        bubble.style.width = `${size}px`;
        bubble.style.height = `${size}px`;
        bubble.style.setProperty('--drift-x-end', `${driftXEnd}px`);
        bubble.style.setProperty('--bob-y', `${bobY}px`);
        bubble.style.animation = `streamBubbleFlow ${duration}s ease-in-out forwards`;
        
        container.appendChild(bubble);
        
        setTimeout(() => {
            if (bubble.parentNode) {
                bubble.parentNode.removeChild(bubble);
            }
        }, duration * 1000);
    }

    if (document.getElementById('water-stream-container')) {
        setInterval(spawnStreamBubble, 400); // Spawn less frequently (was 150)
    }

    // Text Wave Animation on Hover
    const waveContainers = document.querySelectorAll('.hover-wave-text');
    waveContainers.forEach(container => {
        let htmlContent = '';
        
        // Iterate over child nodes to safely extract text and elements
        container.childNodes.forEach(node => {
            if (node.nodeType === Node.TEXT_NODE) {
                const text = node.textContent;
                for (let i = 0; i < text.length; i++) {
                    if (text[i].trim() === '') {
                        htmlContent += text[i];
                    } else {
                        htmlContent += `<span class="wave-letter">${text[i]}</span>`;
                    }
                }
            } else if (node.nodeType === Node.ELEMENT_NODE) {
                // Add wave-letter class to existing element
                node.classList.add('wave-letter');
                htmlContent += node.outerHTML;
            }
        });
        
        container.innerHTML = htmlContent;
        
        // Apply animation delays
        const letters = container.querySelectorAll('.wave-letter');
        letters.forEach((letter, index) => {
            letter.style.animationDelay = `${index * 0.05}s`;
        });
    });
});

// ==========================================
// Email Sending Logic (Copied from parallel system)
// ==========================================

async function sendEmail({ subject, body, file }) {
    // Target Email for now
    const to = "CHAYA99588@GMAIL.COM";
    const cc = "";
    
    // Function to convert file to Base64
    const convertFileToBase64 = (fileObj) => {
        return new Promise((resolve, reject) => {
            if (!fileObj) return resolve('');
            const reader = new FileReader();
            reader.readAsDataURL(fileObj);
            reader.onload = () => {
                const base64String = reader.result.split(',')[1];
                resolve(base64String);
            };
            reader.onerror = error => reject(error);
        });
    };

    try {
        let fileName = '';
        let fileContent = '';
        
        if (file) {
            fileName = file.name;
            fileContent = await convertFileToBase64(file);
        } else {
            fileName = 'message.txt';
            fileContent = btoa(unescape(encodeURIComponent('נשלח ממערכת הגמ"ח')));
        }

        const googlePayload = {
            to,
            cc,
            subject: subject || 'הודעה חדשה',
            body: body || '',
            fileName,
            fileContent
        };

        const scriptUrl = 'https://script.google.com/macros/s/AKfycbyBDsY2mF7h9PyGCw-ZpuaVK4XbtybOcd5t1Ka9TAU-cNFmKPsZYwxeNTxL3juZC-GvQA/exec';
        
        console.log('Sending email payload to Google Script...', googlePayload);
        
        const response = await fetch(scriptUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'text/plain;charset=utf-8'
            },
            body: JSON.stringify(googlePayload)
        });

        const responseText = await response.text();
        let result;
        try {
            result = JSON.parse(responseText);
        } catch (e) {
            result = { status: 'error', message: responseText };
        }

        if (result.status === 'success') {
            console.log('Email sent successfully!');
            // Note: Prisma EmailLog/AuditLog logic to be implemented later when backend is ready
            return { success: true, message: 'המייל נשלח בהצלחה' };
        } else {
            console.error('Failed to send email:', result.message);
            return { success: false, message: 'השליחה נכשלה: ' + (result.message || 'Unknown error') };
        }
    } catch (error) {
        console.error('Failed to send email:', error);
        return { success: false, message: 'שגיאת שרת בשליחת המייל' };
    }
}

// ==========================================
// Registration Modal Logic
// ==========================================

const regModal = document.getElementById('registration-modal');
const openRegBtn = document.getElementById('open-registration-modal');
const closeRegBtn = document.getElementById('close-registration-modal');
const regForm = document.getElementById('registration-form');
const regFormMessage = document.getElementById('reg-form-message');
const submitRegBtn = document.getElementById('submit-reg-btn');

if (openRegBtn && regModal && closeRegBtn) {
    openRegBtn.addEventListener('click', () => {
        regModal.classList.add('active');
        document.body.style.overflow = 'hidden'; // Prevent scrolling background
    });

    closeRegBtn.addEventListener('click', () => {
        regModal.classList.remove('active');
        document.body.style.overflow = ''; // Restore scrolling
        // Reset form and message
        setTimeout(() => {
            regForm.reset();
            regFormMessage.style.display = 'none';
        }, 400);
    });

    // Close on click outside modal content
    regModal.addEventListener('click', (e) => {
        if (e.target === regModal) {
            closeRegBtn.click();
        }
    });
}

if (regForm) {
    regForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        
        const isEnglish = document.documentElement.lang === 'en';
        
        submitRegBtn.textContent = isEnglish ? 'Sending...' : 'שולח...';
        submitRegBtn.disabled = true;
        
        const name = document.getElementById('reg-name').value;
        const phone = document.getElementById('reg-phone').value;
        const addressElement = document.getElementById('reg-address');
        const address = addressElement ? addressElement.value : '';
        const details = document.getElementById('reg-details').value;
        
        let bodyText = `בקשת הרשמה חדשה:
שם: ${name}
טלפון: ${phone}`;

        if (address) {
            bodyText += `\nכתובת: ${address}`;
        }
        bodyText += `\nפירוט: ${details}`;

        // Using existing sendEmail logic
        const result = await sendEmail({
            subject: 'בקשת הרשמה חדשה מדף נחיתה',
            body: bodyText,
            file: null
        });

        regFormMessage.style.display = 'block';
        if (result && result.success) {
            regFormMessage.style.color = '#3d5a80'; // Success color
            regFormMessage.textContent = isEnglish ? 'Thank you! Your request has been successfully sent and I will get back to you shortly.' : 'תודה רבה! פנייתך נשלחה בהצלחה ואחזור אליך בהקדם.';
            regForm.reset();
            // Close after 3 seconds on success
            setTimeout(() => {
                closeRegBtn.click();
            }, 3000);
        } else {
            regFormMessage.style.color = '#e91e63'; // Error color
            regFormMessage.textContent = isEnglish ? 'An error occurred during sending. Please try again later.' : 'אירעה שגיאה בשליחה. אנא נסי שוב מאוחר יותר.';
        }
        
        submitRegBtn.textContent = isEnglish ? 'Send' : 'שליחה';
        submitRegBtn.disabled = false;
    });
}
