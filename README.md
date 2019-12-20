Hack The Box Activity Feed Parser

Utility to download, parse and save the Hack The Box activity feed into a sqlite database
Activity on the feed won't be duplicated, I compare the activity content hash with the values on the db before I insert 

Probably useless to you, but could be useful :}

Development done with Visual Studio Code on Ubuntu

using HtmlAgilityPack for parsing HTML
using Microsoft.EntityFrameworkCore.Sqlite for creating and accessing the sqlite database

build:
dotnet build hackthebox-activity-feed.csproj

run:
dotnet run hackthebox-activity-feed.csproj

HF


