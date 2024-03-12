NPM=../node_modules/.bin
NPMDEPS=$(package-lock.json)
NETDEPS=$(packages.lock.json)

.PHONY: npm dotnet install clean serve test

npm: $(NPMDEPS)
	npm install

dotnet: $(NETDEPS)
	dotnet restore

install: npm dotnet

clean:
	git clean -Xdf

test-android: 
	$(NPM)/percy app:exec --  dotnet run --no-restore --project Android

test-ios: 
	$(NPM)/percy app:exec --  dotnet run --no-restore --project Ios
