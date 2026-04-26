export function validateRequiredText(value, min = 1, max = 100) {
  if (typeof value !== "string") return false;
  const trimmed = value.trim();
  return trimmed.length >= min && trimmed.length <= max;
}

export function validatePositiveNumber(value, min = 0) {
  const parsed = Number(value);
  return !Number.isNaN(parsed) && parsed >= min;
}

export function validateNonNegativeInteger(value) {
  const parsed = Number(value);
  return Number.isInteger(parsed) && parsed >= 0;
}
