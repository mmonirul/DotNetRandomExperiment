---
applyTo: '**/*.cs'
---
Coding standards, domain knowledge, and preferences that AI should follow.

## Namespaces
- Use file-scoped namespaces that match the folder structure.

## Immutability
- Prefer immutable types unless mutability is requested.
- Prefer records over classes for immutable types.

## Files Organization
- Define one type per file.
- Use single class per file if necessary group classed under a relevant namespace.

## Record Design
- Prefer records if the type is not specified. Do not use 'record' if the type is specified as 'class' or 'struct' or asked for create a class.
- Do not use `record class` or `record struct`, always use `record` if not specified.
- ﻿﻿﻿﻿﻿Define record's properties on the same line with the record declaration.
- ﻿﻿﻿﻿﻿Accompany each record `‹name>` with `‹name>Factory` static factory class.
- ﻿﻿﻿﻿﻿Place the factory class in the same file as the record.
- ﻿﻿﻿﻿﻿Expose static `Create` method in the factory class for instantiation.
- ﻿﻿﻿﻿﻿Place argument validation in the `Create` method.
- ﻿﻿﻿﻿﻿Never use record's constructor when there is a factory method.
- ﻿﻿﻿﻿﻿Use immutable collections in records unless requested otherwise.
- ﻿﻿﻿﻿﻿Use `ImmutableList<T>` in records whenever possible and use simplified collection initializers i.e., []. 
- Define record behavior in extension methods in other static classes.|

## Class Design
- Prefer classes for mutable types if not specified.
- Default constructor should be protected.
- Create a static method `New` for instantiation. 
- Verify required arguments in the `New` method.
- Objects should be created using the `New` method.
- Mutation can be done using methods, not properties.

## Discriminated Unions Design
﻿﻿﻿﻿﻿- Prefer using records for discriminated unions.
﻿﻿﻿﻿﻿- Derive specific types from a base abstract record.
﻿﻿﻿﻿﻿- Define the entire discriminated union in one file.
﻿﻿﻿﻿﻿- Define one static factories class per discriminated union.
﻿﻿﻿﻿﻿- Expose one static factory method per variant.
﻿﻿﻿﻿﻿- Follow all rules for record design when designing a discriminated union.