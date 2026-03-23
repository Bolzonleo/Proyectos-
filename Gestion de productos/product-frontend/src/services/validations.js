// validation.js

/**
 * Validate product name
 * @param {string} name - The product name
 * @returns {boolean} - True if valid, false otherwise
 */
function validateProductName(name) {
    return typeof name === 'string' && name.trim().length > 0;
}

/**
 * Validate product price
 * @param {number} price - The product price
 * @returns {boolean} - True if valid, false otherwise
 */
function validateProductPrice(price) {
    return typeof price === 'number' && price >= 0;
}

/**
 * Validate product quantity
 * @param {number} quantity - The product quantity
 * @returns {boolean} - True if valid, false otherwise
 */
function validateProductQuantity(quantity) {
    return Number.isInteger(quantity) && quantity >= 0;
}

export { validateProductName, validateProductPrice, validateProductQuantity };