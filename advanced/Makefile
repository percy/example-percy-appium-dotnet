NPM=node_modules/.bin

.PHONY: install clean test test-advanced

$(NPM):
	npm install --no-save @percy/cli@^1.31.13

install: $(NPM)
	dotnet restore

clean:
	rm -rf bin obj node_modules

# Run against BrowserStack App Automate hub. Requires AA_USERNAME,
# AA_ACCESS_KEY, APP, PERCY_TOKEN env vars.
test test-advanced: install
	$(NPM)/percy app:exec -- dotnet test
