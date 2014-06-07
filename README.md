# Wikipedia Loader

A project for playing with loading Wikipedia data dumps.
The Wikipedia monthly dumps can be found at (http://dumps.wikimedia.org/enwiki/).

## Background

The main article (page) dump is around 45 GB of data as a single XML file.
This project is using a buffered stream to read the file in chunks of one page element at a time.
It then loads that page XML and writes it out to an equivalent JSON format (doing a "dumb" conversion).

In the current implementation, the JSON document has a root "page" element inside each file.
It is probably unnecessary by itself, but it is left there for now in case it makes sense to blindly recombine the pages together using simple file I/O operations.