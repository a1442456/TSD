module.exports = {
  "root": true,
  "env": {
    "browser": true,
  },
  "parser": "@typescript-eslint/parser",
  "parserOptions": {
    "tsconfigRootDir": __dirname,
    "project": ["./packages/**/tsconfig.json"],
    "impliedStrict": true,
    "ecmaFeatures": {
      "jsx": true
    }
  },
  "ignorePatterns": [".eslintrc.js"],
  "plugins": [
    "react",
    "@typescript-eslint",
    "prettier"
  ],
  "extends": [
    "eslint:recommended",
    "plugin:react/recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:@typescript-eslint/recommended-requiring-type-checking",
    // "plugin:prettier/recommended",
    "prettier"
  ],
  "rules": {
    "@typescript-eslint/no-empty-interface": "off",
    "@typescript-eslint/no-explicit-any": "off",
    "@typescript-eslint/explicit-module-boundary-types": "off",
    "@typescript-eslint/no-inferrable-types": "off",
    "react/display-name": "off",
    "sort-imports": "off"
  },
  "settings": {
    "react": {
      "version": "detect"
    }
  }
};
