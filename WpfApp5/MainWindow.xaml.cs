﻿using System;
using System.Windows;
using System.Windows.Data;
using System.Data.Entity;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Linq;

namespace WpfApp5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Model.EFContext db = (Application.Current as App).db;

        CollectionViewSource studentViewSource;
        CollectionViewSource groupViewSource;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            db.Groups.Load();
            db.Students.Load();

            studentViewSource = ((CollectionViewSource)(this.FindResource("studentViewSource")));
            studentViewSource.Source = db.Students.Local;

            groupViewSource = (CollectionViewSource)this.FindResource("groupViewSource");
            groupViewSource.Source = db.Groups.Local;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var db = (Application.Current as App).db;
            db.SaveChanges();
            if (db.HasChanges)
            {
                int changes = 0;
                try
                {
                    changes = db.SaveChanges();
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    MessageBox.Show($"{changes} изменений сохранено", "Уведомление");
                }
            }
        }

        private void ContentControl_LostFocus(object sender, RoutedEventArgs e)
        {
            studentViewSource.View.Refresh();
            groupViewSource.View.Refresh();
        }

        private void AddStudent_Click(object sender, EventArgs e)
        {
            if (db.Groups.Local.Count == 0)
            {
                MessageBox.Show("Создайте группу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
    

            var newStudentDialog = new NewStudent();
            if (newStudentDialog.ShowDialog() == true)
            {
                var student = (Model.Student)(newStudentDialog.FindResource("studentViewSource"));
                db.Students.Add(student);
            }           
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            var removedStudent = studentsList.SelectedItem as Model.Student;
            db.Students.Remove(removedStudent);
        }


        private void AddGroup_Click(object sender, EventArgs e)
        {
            var newGroupDialog = new NewGroup();
            if (newGroupDialog.ShowDialog() == true)
            {
                var group = (Model.Group)(newGroupDialog.FindResource("groupViewSource"));
                db.Groups.Add(group);
            }
        }

        private void RemoveGroup_Click(object sender, EventArgs e)
        {
            var removedGroup = groupsList.SelectedItem as Model.Group;
            db.Groups.Remove(removedGroup);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var newExportDialog = new Message();

            if (newExportDialog.ShowDialog() == true)
            {
                if (newExportDialog.DialogResult == true) 
                {

                    SaveFileDialog saveDialog = new SaveFileDialog();
                    if (saveDialog.ShowDialog() == true)
                    {
                        var studentViewSource = ((CollectionViewSource)(this.FindResource("studentViewSource")));
                        var studentsCollection = studentViewSource.Source as ObservableCollection<Model.Student>;

                        var currentGroup = newExportDialog.Group;

                        var data = from student in studentsCollection
                                   where student.Group == currentGroup
                                   select student;

                        var export = new StudentsToXLSProvider(data);

                        export.ExportTo($"{saveDialog.FileName}.xlsx");
                        MessageBox.Show($"Экспортировано в файл {saveDialog.FileName}", "Уведомление");
                    }
                }
            }
        }
    }
}