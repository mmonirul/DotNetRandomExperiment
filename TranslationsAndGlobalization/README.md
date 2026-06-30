# .NET 8 Localization Example

This is a console application that demonstrates localization (i18n) in .NET 8 using multiple languages.

## Features

- **Multi-language support**: English (default/fallback), Swedish, and Spanish
- **Resource files**: Uses `.resx` files for translation management
- **Microsoft.Extensions.Localization**: Leverages the built-in localization framework
- **User interaction**: Prompts user for language selection
- **Culture-specific formatting**: Demonstrates culture-aware time formatting

## Supported Languages

- **en** - English (Default and fallback language)
- **sv** - Swedish (svenska)
- **es** - Spanish (español)

## Resource Keys

The application includes three localized strings:

1. **Welcome** - Welcome message
2. **CurrentTime** - Current time display with parameter
3. **Goodbye** - Farewell message

## Project Structure

```
LocalizationExample/
├── Program.cs                          # Main application entry point
├── LocalizationExample.csproj         # Project file with dependencies
├── Services/
│   ├── LocalizationService.cs         # Service for handling localization
│   ├── LocalizationService.resx       # English resources for service
│   ├── LocalizationService.sv.resx    # Swedish resources for service
│   └── LocalizationService.es.resx    # Spanish resources for service
└── Resources/
    ├── Messages.resx                   # English resources (default)
    ├── Messages.sv.resx                # Swedish resources
    ├── Messages.es.resx                # Spanish resources
    └── Messages.Designer.cs            # Generated strongly-typed resource class
```

## How to Run

1. **Build the project**:

   ```bash
   dotnet build
   ```

2. **Run the application**:

   ```bash
   dotnet run
   ```

3. **Follow the prompts**:
   - Enter a language code: `en`, `sv`, or `es`
   - View the localized messages
   - Type `exit` to quit

## Example Usage

```
=== .NET 8 Localization Example ===
Supported languages:
- en (English) - Default
- sv (Swedish)
- es (Spanish)

Enter language code (en/sv/es) or 'exit' to quit: sv

--- Swedish ---
Välkommen till vår applikation!
Nuvarande tid är: 2025-07-06 14:30:25
Tack för att du använder vår applikation!

--- Additional demonstration using ResourceManager ---
Welcome: Välkommen till vår applikation!
Current Time: Nuvarande tid är: 2025-07-06 14:30:25
Goodbye: Tack för att du använder vår applikation!

Enter language code (en/sv/es) or 'exit' to quit: es

--- Spanish ---
¡Bienvenido a nuestra aplicación!
La hora actual es: 2025-07-06 14:30:25
¡Gracias por usar nuestra aplicación!
```

## Dependencies

- **.NET 8.0**: Target framework
- **Microsoft.Extensions.Localization**: Core localization functionality
- **Microsoft.Extensions.Hosting**: Hosting and dependency injection
- **Microsoft.Extensions.DependencyInjection**: Service container

## Key Features Demonstrated

1. **Resource File Management**: Shows how to organize translations in `.resx` files
2. **Culture Setting**: Demonstrates setting `CultureInfo.CurrentCulture` and `CultureInfo.CurrentUICulture`
3. **Dependency Injection**: Uses DI container to manage localization services
4. **Strongly-Typed Resources**: Shows both `IStringLocalizer` and generated resource classes
5. **Fallback Mechanism**: English serves as the default fallback language
6. **Parameter Formatting**: Demonstrates localized string formatting with parameters

## Adding New Languages

To add a new language (e.g., French):

1. Create new resource files:

   - `Resources/Messages.fr.resx`
   - `Services/LocalizationService.fr.resx`

2. Add the language option to the `Program.cs` switch statement

3. Update the help text to include the new language option

## Notes

- The application uses both `IStringLocalizer` and direct `ResourceManager` access to demonstrate different approaches
- English is set as the fallback language, so any missing translations will fall back to English
- The application handles invalid culture codes gracefully
- Time formatting respects the selected culture's conventions
