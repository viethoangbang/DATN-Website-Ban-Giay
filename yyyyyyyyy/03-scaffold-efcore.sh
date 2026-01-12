#!/bin/bash

echo "==================================="
echo "Scaffolding EF Core Models"
echo "==================================="

DB_NAME="SneakerShop"
SA_PASSWORD="@AAAaaa123123"
CONNECTION_STRING="Server=localhost,1433;Database=$DB_NAME;User Id=SA;Password=$SA_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"

DATA_PROJECT_DIR="../Backend/Data"
OUTPUT_DIR="Models"
CONTEXT_NAME="SneakerShopContext"

# Check if Data project directory exists
if [ ! -d "$DATA_PROJECT_DIR" ]; then
    echo "❌ Data project directory not found: $DATA_PROJECT_DIR"
    exit 1
fi

cd "$DATA_PROJECT_DIR"

echo "Installing/Updating EF Core tools..."
dotnet tool install --global dotnet-ef 2>/dev/null || dotnet tool update --global dotnet-ef

echo ""
echo "Scaffolding models to: $OUTPUT_DIR"

# Remove old models if exists
if [ -d "$OUTPUT_DIR" ]; then
    echo "Removing old models..."
    rm -rf "$OUTPUT_DIR"
fi

# Remove old context if exists
echo "Removing old context..."
rm -f *Context.cs 2>/dev/null

# Scaffold
dotnet ef dbcontext scaffold "$CONNECTION_STRING" \
    Microsoft.EntityFrameworkCore.SqlServer \
    --output-dir "$OUTPUT_DIR" \
    --context "$CONTEXT_NAME" \
    --force \
    --no-onconfiguring \
    --data-annotations

if [ $? -eq 0 ]; then
    echo ""
    echo "✅ Scaffolding completed successfully!"
    echo ""
    echo "Generated files:"
    echo "  Context: $CONTEXT_NAME.cs"
    echo "  Models: $OUTPUT_DIR/"
    if [ -d "$OUTPUT_DIR" ]; then
        ls -1 "$OUTPUT_DIR" | sed 's/^/    - /'
    fi
    echo ""
    echo "Location: Backend/Data/"
    echo "🎉 All done! Your database is ready to use."
else
    echo ""
    echo "❌ Scaffolding failed!"
    echo ""
    echo "Common issues:"
    echo "  1. Make sure Backend project has these packages:"
    echo "     - Microsoft.EntityFrameworkCore.SqlServer"
    echo "     - Microsoft.EntityFrameworkCore.Design"
    echo "  2. Install them with:"
    echo "     dotnet add package Microsoft.EntityFrameworkCore.SqlServer"
    echo "     dotnet add package Microsoft.EntityFrameworkCore.Design"
    exit 1
fi
