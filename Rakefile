require 'rubygems'
require "bundler/setup"
require 'fileutils'
require 'albacore'
require 'git'


$dir=Dir.pwd.gsub("/","\\")
$testdb="Data Source=.\\SQLEXPRESS;AttachDbFilename=#{$dir}\\Charcoal.DataLayer.Tests\\TestDatabase.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True"
$proddb="Data Source=.\\SQLEXPRESS;AttachDbFilename=#{$dir}\\Charcoal.Web\\App_Data\\Local.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True"

def ouputToConsole(str)
puts "Operator>>>>>>>",str
end

def migrate(migrator, filePath, connection, task)
	migrator.command = "packages/FluentMigrator.1.0.2.0/tools/Migrate.exe"
	migrator.provider = 'sqlserver'
	migrator.target = filePath
	migrator.connection = connection
	migrator.verbose = true
	migrator.task = task
end

def pullChanges(dirPath)
	git = Git.open (dirPath)
	statuses=git.status
	
	if(statuses.changed.count > 0 or 
	   statuses.added.count > 0 or
	   statuses.deleted.count > 0)
		ouputToConsole "You have uncommitted changes"
		statuses.changed.each do |stat|
			ouputToConsole "#{stat[0]} has been changed"
		end
		statuses.added.each do |stat|
			ouputToConsole "#{stat[0]} has been added"
		end
		statuses.deleted.each do |stat|
			ouputToConsole "#{stat[0]} has been deleted"
		end
		#Alert user he has untracked changes?
		#statuses.untracked.each do |stat|
		#	puts "#{stat[0]} is not yet tracked"
		#end
		raise "Please check your Git Status, Commit or Checkout changes and then try again"
	end	
	puts git.pull
end

task :default => [:build]

desc "Build"
msbuild :build do |msb|
	msb.properties :configuration => :Debug
	msb.targets [ :Rebuild ]
	msb.solution = "Charcoal.sln"
end

desc "Run Tests"
nunit :unit => :build do |nunit|
	nunit.command = "packages/NUnit.2.5.10.11092/tools/nunit-console.exe"
	nunit.assemblies "Charcoal.DataLayer.Tests/bin/Debug/Charcoal.DataLayer.Tests.dll"
end

desc "Migrate Database Up. example rake migrate['prod']"
fluentmigrator :migrate, [:db] => :build do |migrator, args|
	#migrate local test database
	connection = args[:db] == 'prod' ? $proddb : $testdb
	migrate migrator, "Charcoal.Migrations/bin/Debug/Charcoal.Migrations.dll", connection, 'migrate'
	#migrate server database
end

desc "Pull changes"
task :pull do	
	pullChanges(".")
end

desc "Migrate Database Down"
fluentmigrator :rollback, [:db] do |migrator, args|
	#migrate local test database
	connection = args[:db] == 'prod' ? $proddb : $testdb
	migrate migrator, "Charcoal.Migrations/bin/Debug/Charcoal.Migrations.dll", connection, 'rollback'
	#migrate server database
end

desc "Ready to checkin"
task :checkin => [:pull,:migrate, :unit] do
	begin
	ouputToConsole "You should be ready to checkin"
	git = Git.open (".")
	puts git.push
	ouputToConsole "Checkin completed successfully :D"
	rescue
	ouputToConsole "Sorry Dude (or Gal), you're not ready to checkin yet :("
	end
end