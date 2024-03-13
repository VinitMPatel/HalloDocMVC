﻿using Data.Entity;
using Microsoft.AspNetCore.Http;
using Services.ViewModels;

namespace Services.Contracts
{
    public interface IDashboardData
    {
        AdminDashboard AllStateData(String a , String b , int p , string searchKey);

        AdminDashboard AllData();

        CaseActionDetails ViewCaseData(int requestId);

        List<Physician> PhysicianList(int regionid);

        CaseActionDetails ViewUploads(int requestId);
        void UplodingDocument(List<IFormFile> myfile, int reqid);
        void uploadFile(List<IFormFile> upload, int id);
        void SingleDelete(int reqfileid);
    }
}