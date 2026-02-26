#!/bin/bash
# Script to add 'area:faulty-device' label to identified issues
# Run this script in the repository root with GitHub CLI (gh) installed and authenticated

echo "Adding 'area:faulty-device' label to identified imported issues..."
echo ""

# Array of issue numbers that need the label
issues_to_tag=(62 89 101 370 436 475)

for issue in "${issues_to_tag[@]}"; do
    echo "Processing issue #$issue..."
    gh issue edit $issue --add-label "area:faulty-device"
    if [ $? -eq 0 ]; then
        echo "✓ Successfully added label to issue #$issue"
    else
        echo "✗ Failed to add label to issue #$issue"
    fi
    echo ""
done

# Also add 'imported' label to #101 which is missing it
echo "Adding 'imported' label to issue #101..."
gh issue edit 101 --add-label "imported"
if [ $? -eq 0 ]; then
    echo "✓ Successfully added 'imported' label to issue #101"
else
    echo "✗ Failed to add 'imported' label to issue #101"
fi

echo ""
echo "Done! All labels have been processed."
echo ""
echo "Summary:"
echo "- Added 'area:faulty-device' label to 6 issues: #62, #89, #101, #370, #436, #475"
echo "- Added 'imported' label to issue #101"
