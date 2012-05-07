require 'albacore'


def migrate(migrator, filePath, connection, task)
	migrator.command = "packages/FluentMigrator.1.0.2.0/tools/Migrate.exe"
	migrator.provider = 'sqlserver'
	migrator.target = filePath
	migrator.connection = connection
	migrator.verbose = true
	migrator.task = task
end

task :default => [:build]

desc "Build"
msbuild :build do |msb|
	msb.properties :configuration => :Release
	msb.targets :Clean, :Build
	msb.solution = "Charcoal.sln"
end

desc "Run Tests"
nunit :unit => :build do |nunit|
	nunit.command = "packages/NUnit.2.5.10.11092/tools/nunit-console.exe"
	nunit.assemblies "Charcoal.DataLayer.Tests/bin/Debug/Charcoal.DataLayer.Tests.dll"
end


desc "Migrate Database Up"
fluentmigrator :migrate do |migrator|
	#migrate local test database
	dir=Dir.pwd.gsub("/","\\")
	migrate migrator, "Charcoal.Migrations/bin/Debug/Charcoal.Migrations.dll", "Data Source=.\\SQLEXPRESS;AttachDbFilename=#{dir}\\Charcoal.DataLayer.Tests\\TestDatabase.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True",'migrate'
	#migrate server database
end

desc "Migrate Database Down"
fluentmigrator :rollback do |migrator|
	#migrate local test database
	dir=Dir.pwd.gsub("/","\\")
	migrate migrator, "Charcoal.Migrations/bin/Debug/Charcoal.Migrations.dll", "Data Source=.\\SQLEXPRESS;AttachDbFilename=#{dir}\\Charcoal.DataLayer.Tests\\TestDatabase.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True",'rollback'
	#migrate server database
end