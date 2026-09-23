# Публикация проекта и командная работа

## 1. Клонирование подготовленной истории

Если рядом с проектом получен файл `GeometryDrawingLab.bundle`, восстановите репозиторий так:

```powershell
git clone GeometryDrawingLab.bundle GeometryDrawingLab
cd GeometryDrawingLab
git branch feature/rectangle origin/feature/rectangle
git branch feature/move-shapes origin/feature/move-shapes
git branch feature/figure-styling origin/feature/figure-styling
git switch main
```

Проверьте историю и ветки:

```powershell
git log --graph --oneline --decorate --all
git branch -a
```

## 2. Укажите настоящего автора новых коммитов

Каждый участник выполняет команды со своими данными:

```powershell
git config user.name "Имя Фамилия"
git config user.email "почта-из-профиля-github@example.com"
```

Подготовительные коммиты имеют нейтрального автора `Geometry Lab Team`; новые коммиты и Pull Request должны выполняться из личных аккаунтов участников.

## 3. Создайте пустой репозиторий на GitHub

Не добавляйте через GitHub README, `.gitignore` или лицензию: они уже есть в проекте. Затем подключите удалённый репозиторий:

```powershell
git remote set-url origin https://github.com/USERNAME/GeometryDrawingLab.git
git push -u origin main
git push -u origin feature/rectangle
git push -u origin feature/move-shapes
git push -u origin feature/figure-styling
```

Если используется SSH, замените URL на `git@github.com:USERNAME/GeometryDrawingLab.git`.

## 4. Работа первого участника: перемещение

1. На GitHub откройте **Pull requests → New pull request**.
2. Выберите `base: main` и `compare: feature/move-shapes`.
3. Заголовок: `Добавлено перемещение фигур по холсту`.
4. В описание вставьте содержимое шаблона Pull Request.
5. Второй участник проверяет вкладку **Files changed**, запускает приложение и оставляет review.
6. Выберите **Create a merge commit**, затем **Merge pull request**.
7. Обновите локальную ветку:

```powershell
git switch main
git pull origin main
```

Не сливайте `feature/move-shapes` локально до Pull Request: иначе GitHub не увидит разницу между ветками.

Чтобы в истории был личный программный коммит первого участника, он должен проверить ветку и сделать небольшое настоящее улучшение — например, изменить шаг кнопок перемещения с 10 на 15 пикселей в `MainWindow.xaml`, затем выполнить:

```powershell
git add src/GeometryDrawing.Wpf/MainWindow.xaml
git commit -m "feat(move): adjust movement step"
git push
```

## 5. Работа второго участника: оформление и клавиатура

После слияния первого Pull Request второй участник выполняет:

```powershell
git fetch origin
git switch feature/figure-styling
git merge origin/main
```

В этой ветке уже подготовлен существенный самостоятельный блок:

- выбор одного из пяти цветов линий;
- регулировка толщины от 1 до 10;
- перемещение стрелками и W/A/S/D;
- отображение координат вершин на холсте.

Второй участник проверяет функциональность и делает собственное улучшение. Например, добавляет в `LineColorComboBox` новый оранжевый вариант и соответствующую ветку `DarkOrange` в методе `FigureStyle_Changed`. Затем:

```powershell
git add src/GeometryDrawing.Wpf/MainWindow.xaml src/GeometryDrawing.Wpf/MainWindow.xaml.cs
git commit -m "feat(style): add orange line color"
git push
```

На GitHub создаётся второй Pull Request:

- `base: main`;
- `compare: feature/figure-styling`;
- заголовок: `Добавлены стили фигур и управление с клавиатуры`.

Первый участник выполняет review, после чего PR сливается через **Create a merge commit**.

## 6. Распределение работы

| Участник | Задача | Ветка |
| --- | --- | --- |
| Первый | Перемещение по осям, проверка границ | `feature/move-shapes` |
| Второй | Цвет, толщина, клавиатура, координаты | `feature/figure-styling` |

Для новой задачи:

```powershell
git switch main
git pull
git switch -c feature/короткое-название
```

После одного логически завершённого изменения:

```powershell
git add путь-к-изменённым-файлам
git commit -m "feat: кратко описать результат"
git push -u origin feature/короткое-название
```

Хороший коммит содержит один законченный смысловой шаг. Не объединяйте в одном коммите рефакторинг, новый функционал и форматирование.

## 7. История, подготовленная в проекте

Последовательность коммитов показывает этапы лабораторной:

1. каркас WPF-проекта и Canvas;
2. класс `Point2D`;
3. класс `Triangle`;
4. случайный треугольник и отрисовка;
5. класс `RectangleFigure` в отдельной ветке;
6. случайный прямоугольник;
7. merge ветки `feature/rectangle`;
8. ручной ввод и квадрат;
9. безопасное перемещение в ветке `feature/move-shapes`;
10. элементы управления перемещением;
11. цвет и толщина линий в `feature/figure-styling`;
12. управление клавиатурой и отображение координат.
