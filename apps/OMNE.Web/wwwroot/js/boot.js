(function () {
  const icons = Array.from(document.fonts)
    .filter(font => font.family.startsWith("Material Symbols"))
    .map(x => x.load());

  // hide icons until font loaded
  if (icons.length) {
    const style = document.createElement("style");
    style.textContent = `i { visibility: hidden !important; }`;
    document.head.appendChild(style);
    Promise.all(icons).then(() => style.remove());
  }
})();

(function () {
  Blazor.start();
})();
