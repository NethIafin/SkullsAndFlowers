// wwwroot/js/arrowUtils.js
window.arrowPositions = window.arrowPositions || [];

window.setupArrowResizeHandler = function () {
    // Remove existing handlers to avoid duplicates
    window.removeEventListener('resize', window.updateAllArrows);

    // Add resize handler
    window.addEventListener('resize', window.updateAllArrows);

    // Add MutationObserver to detect DOM changes
    if (window.arrowMutationObserver) {
        window.arrowMutationObserver.disconnect();
    }

    window.arrowMutationObserver = new MutationObserver(function() {
        window.updateAllArrows();
    });

    window.arrowMutationObserver.observe(document.body, {
        childList: true,
        subtree: true,
        attributes: true
    });

    console.log("Arrow resize handler setup complete");
};

window.updateAllArrows = function () {
    if (window.arrowUpdateTimeout) {
        clearTimeout(window.arrowUpdateTimeout);
    }

    window.arrowUpdateTimeout = setTimeout(function() {
        if (window.arrowPositions && window.arrowPositions.length > 0) {
            window.arrowPositions.forEach(function(arrow) {
                try {
                    updateArrow(arrow.id, arrow.sourceId, arrow.targetId, arrow.text);
                } catch (e) {
                    console.error('Error updating arrow:', e);
                }
            });
        }
    }, 50);
};

window.updateArrow = function (arrowId, sourceId, targetId, text) {
    // Find elements
    const sourceElement = document.getElementById(sourceId);
    const targetElement = document.getElementById(targetId);

    if (!sourceElement || !targetElement) {
        console.error(`Elements not found: ${sourceId} or ${targetId}`);
        return;
    }

    const arrowSvg = document.getElementById(`arrow-${arrowId}`);
    const arrowPath = document.getElementById(`arrow-path-${arrowId}`);
    const arrowText = document.getElementById(`arrow-text-${arrowId}`);
    const arrowTextBg = document.getElementById(`arrow-text-bg-${arrowId}`);

    if (!arrowPath || !arrowText || !arrowTextBg) {
        console.error(`Arrow elements not found for: ${arrowId}`);
        return;
    }

    // Get the page scroll position
    const scrollX = window.scrollX || window.pageXOffset;
    const scrollY = window.scrollY || window.pageYOffset;

    // Get element positions relative to the viewport
    const sourceRect = sourceElement.getBoundingClientRect();
    const targetRect = targetElement.getBoundingClientRect();

    // Calculate center points of the elements
    const sourceX = sourceRect.left + scrollX + (sourceRect.width / 2);
    const sourceY = sourceRect.top + scrollY + (sourceRect.height / 2);
    const targetX = targetRect.left + scrollX + (targetRect.width / 2);
    const targetY = targetRect.top + scrollY + (targetRect.height / 2);

    // Update path
    arrowPath.setAttribute("d", `M${sourceX},${sourceY} L${targetX},${targetY}`);

    // Update text and background
    const midX = (sourceX + targetX) / 2;
    const midY = (sourceY + targetY) / 2;

    // CRITICAL: Force display to be visible ALWAYS if text exists
    if (text && text.trim() !== "") {
        // Set text content and position
        arrowText.textContent = text;
        arrowText.setAttribute("x", midX);
        arrowText.setAttribute("y", midY);

        // CRITICAL: Force style to be visible
        arrowText.style.display = "block";
        arrowTextBg.style.display = "block";

        // Remove any display attribute that might be set
        arrowText.removeAttribute("display");
        arrowTextBg.removeAttribute("display");

        // Calculate text dimensions for background (approximate)
        const textWidth = Math.max(text.length * 8, 40); // Minimum width
        const textHeight = 22;

        // Update text background
        arrowTextBg.setAttribute("x", midX - (textWidth / 2) - 5);
        arrowTextBg.setAttribute("y", midY - (textHeight / 2));
        arrowTextBg.setAttribute("width", textWidth + 10);
        arrowTextBg.setAttribute("height", textHeight);

        console.log(`Arrow ${arrowId} text display explicitly set to: block`);
    } else {
        // Hide text elements
        arrowText.style.display = "none";
        arrowTextBg.style.display = "none";
    }

    // Store arrow position for future updates
    const existingArrowIndex = window.arrowPositions.findIndex(a => a.id === arrowId);
    const arrowData = { id: arrowId, sourceId, targetId, text };

    if (existingArrowIndex >= 0) {
        window.arrowPositions[existingArrowIndex] = arrowData;
    } else {
        window.arrowPositions.push(arrowData);
    }
};