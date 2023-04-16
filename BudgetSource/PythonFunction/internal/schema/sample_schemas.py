class TextDocument:
    def __init__(self, title: str, content: str):
        self._title = title
        self._content = content

    @property
    def title(self) -> str:
        return self._title

    @title.setter
    def title(self, value: str):
        self._title = value

    @property
    def content(self) -> str:
        return self._content

    @content.setter
    def content(self, value: str):
        self._content = value


class WordCount:
    def __init__(self, original_content: str, word_count: int):
        self._original_content = original_content
        self._word_count = word_count

    @property
    def original_content(self) -> str:
        return self._original_content

    @original_content.setter
    def original_content(self, value: str):
        self._original_content = value

    @property
    def word_count(self) -> int:
        return self._word_count

    @word_count.setter
    def word_count(self, value: int):
        self._word_count = value


class AllCaps:
    def __init__(self, original_content: str, all_caps_content: str, transformed_count: int):
        self._original_content = original_content
        self._all_caps_content = all_caps_content
        self._transformed_count = transformed_count

    @property
    def original_content(self) -> str:
        return self._original_content

    @original_content.setter
    def original_content(self, value: str):
        self._original_content = value

    @property
    def all_caps_content(self) -> str:
        return self._all_caps_content

    @all_caps_content.setter
    def all_caps_content(self, value: str):
        self._all_caps_content = value

    @property
    def transformed_count(self) -> int:
        return self._transformed_count

    @transformed_count.setter
    def transformed_count(self, value: int):
        self._transformed_count = value
