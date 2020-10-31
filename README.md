# MarkdownToJson converter

This is a .NET Core console application that will convert a Markdown file to a parsable JSON format. It understand most Markdown extensions as well as a few from https://docs.microsoft.com.

## Usage

```
MDToJson {filename} or {wildcard} or {folder}

If multiple files are matched, a JSON array will be returned, otherwise it will be a single object representing the Markdown file.
```

## Sample input

```markdown
# Header

Paragraphs are separated by a blank line.

2nd paragraph. *Italic*, **bold**, and `monospace`. Itemized lists
look like:

* this one
* that one
* the other one

> Block quotes are written like so.
>
> They can span multiple paragraphs,
> if you like.

and images look like this: ![sample image](https://avatars0.githubusercontent.com/u/5099741?s=460&u=a543dfc724057cda2f7f73fc66adb14b489c5b14&v=4)

## Sub header

Here's a numbered list:

1. first item
2. second item
3. third item

```python
import time
# count to ten
for i in range(10):
    time.sleep(0.5)
    print(i)
```

### Nested header

A nested list:

1. First, get these ingredients:
  * carrots
  * celery
  * lentils

1. Boil some water.

1. Dump everything in the pot and follow these steps:
  - find wooden spoon
  - uncover pot
  - stir
  - cover pot
  - balance wooden spoon precariously on pot handle
  - wait 10 minutes
  - goto first step (or shut off burner when done)

Here's a link to [a website](https://github.com/markjulmar) and to a [section heading in the current
doc](#nested-header).

Here's a table

| Name | Size | Material | Color |
|------|------|----------|-------|
| All Business | 9  | Leather     | Brown       |
| Roundabout   | 10 | Canvas      | Natural     |
| Cinderella   | 11 | Glass       | Transparent |
```

## Sample output

```json
{
  "filename": "sample.md",
  "document": {
    "entries": [
      {
        "type": "heading",
        "level": "1",
        "value": "Header"
      },
      {
        "type": "paragraph",
        "items": [
          {
            "type": "text",
            "value": "Paragraphs are separated by a blank line."
          }
        ]
      },
      {
        "type": "paragraph",
        "items": [
          {
            "type": "text",
            "value": "2nd paragraph. "
          },
          {
            "type": "text",
            "bold": "false",
            "italic": "true",
            "value": "Italic"
          },
          {
            "type": "text",
            "value": ", "
          },
          {
            "type": "text",
            "bold": "true",
            "italic": "false",
            "value": "bold"
          },
          {
            "type": "text",
            "value": ", and "
          },
          {
            "type": "code",
            "value": "monospace"
          },
          {
            "type": "text",
            "value": ". Itemized lists"
          },
          {
            "type": "linebreak",
            "subtype": "soft"
          },
          {
            "type": "text",
            "value": "look like:"
          }
        ]
      },
      {
        "type": "bulleted-list",
        "items": [
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "this one"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "that one"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "the other one"
              }
            ]
          }
        ]
      },
      {
        "type": "quote",
        "value": [
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "Block quotes are written like so."
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "They can span multiple paragraphs,"
              },
              {
                "type": "linebreak",
                "subtype": "soft"
              },
              {
                "type": "text",
                "value": "if you like."
              }
            ]
          }
        ]
      },
      {
        "type": "paragraph",
        "items": [
          {
            "type": "text",
            "value": "and images look like this: "
          },
          {
            "type": "image",
            "url": "https://avatars0.githubusercontent.com/u/5099741?s=460\u0026amp;u=a543dfc724057cda2f7f73fc66adb14b489c5b14\u0026amp;v=4",
            "title": "sample image"
          }
        ]
      },
      {
        "type": "heading",
        "level": "2",
        "value": "Sub header"
      },
      {
        "type": "paragraph",
        "items": [
          {
            "type": "text",
            "value": "Here\u0027s a numbered list:"
          }
        ]
      },
      {
        "type": "ordered-list",
        "start": "1",
        "items": [
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "first item"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "second item"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "third item"
              }
            ]
          }
        ]
      },
      {
        "type": "code",
        "language": "python",
        "lines": [
          "import time",
          "# count to ten",
          "for i in range(10):",
          "    time.sleep(0.5)",
          "    print(i)"
        ]
      },
      {
        "type": "heading",
        "level": "3",
        "value": "Nested header"
      },
      {
        "type": "paragraph",
        "items": [
          {
            "type": "text",
            "value": "A nested list:"
          }
        ]
      },
      {
        "type": "ordered-list",
        "start": "1",
        "items": [
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "First, get these ingredients:"
              }
            ]
          }
        ]
      },
      {
        "type": "bulleted-list",
        "items": [
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "carrots"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "celery"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "lentils"
              }
            ]
          }
        ]
      },
      {
        "type": "ordered-list",
        "start": "1",
        "items": [
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "Boil some water."
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "Dump everything in the pot and follow these steps:"
              }
            ]
          }
        ]
      },
      {
        "type": "bulleted-list",
        "items": [
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "find wooden spoon"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "uncover pot"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "stir"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "cover pot"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "balance wooden spoon precariously on pot handle"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "wait 10 minutes"
              }
            ]
          },
          {
            "type": "paragraph",
            "items": [
              {
                "type": "text",
                "value": "goto first step (or shut off burner when done)"
              }
            ]
          }
        ]
      },
      {
        "type": "paragraph",
        "items": [
          {
            "type": "text",
            "value": "Here\u0027s a link to "
          },
          {
            "type": "link",
            "url": "https://github.com/markjulmar",
            "title": "a website"
          },
          {
            "type": "text",
            "value": " and to a "
          },
          {
            "type": "link",
            "url": "#nested-header",
            "title": "section heading in the current"
          },
          {
            "type": "text",
            "value": "."
          }
        ]
      },
      {
        "type": "paragraph",
        "items": [
          {
            "type": "text",
            "value": "Here\u0027s a table"
          }
        ]
      },
      {
        "type": "table",
        "rows": [
          {
            "row-id": "1",
            "isHeader": "true",
            "columns": [
              {
                "col-id": "1",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Name"
                    }
                  ]
                }
              },
              {
                "col-id": "2",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Size"
                    }
                  ]
                }
              },
              {
                "col-id": "3",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Material"
                    }
                  ]
                }
              },
              {
                "col-id": "4",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Color"
                    }
                  ]
                }
              }
            ]
          },
          {
            "row-id": "2",
            "columns": [
              {
                "col-id": "1",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "All Business"
                    }
                  ]
                }
              },
              {
                "col-id": "2",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "9"
                    }
                  ]
                }
              },
              {
                "col-id": "3",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Leather"
                    }
                  ]
                }
              },
              {
                "col-id": "4",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Brown"
                    }
                  ]
                }
              }
            ]
          },
          {
            "row-id": "3",
            "columns": [
              {
                "col-id": "1",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Roundabout"
                    }
                  ]
                }
              },
              {
                "col-id": "2",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "10"
                    }
                  ]
                }
              },
              {
                "col-id": "3",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Canvas"
                    }
                  ]
                }
              },
              {
                "col-id": "4",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Natural"
                    }
                  ]
                }
              }
            ]
          },
          {
            "row-id": "4",
            "columns": [
              {
                "col-id": "1",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Cinderella"
                    }
                  ]
                }
              },
              {
                "col-id": "2",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "11"
                    }
                  ]
                }
              },
              {
                "col-id": "3",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Glass"
                    }
                  ]
                }
              },
              {
                "col-id": "4",
                "contents": {
                  "type": "paragraph",
                  "items": [
                    {
                      "type": "text",
                      "value": "Transparent"
                    }
                  ]
                }
              }
            ]
          }
        ]
      }
    ]
  }
}
```
